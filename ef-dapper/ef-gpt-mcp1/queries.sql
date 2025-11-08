-- Question: Which users currently have overdue loans and how much unpaid fines do they have?
-- Why this is useful:
--   This helps library staff prioritize outreach and collection efforts by identifying
--   users who have overdue items and outstanding unpaid fines. Combining both metrics
--   (count of overdue items and total unpaid fines) helps rank cases by severity.

-- CTE-based query (SQLite):
WITH OverdueLoans AS (
    SELECT
        l.LoanID,
        l.UserID,
        l.CopyID,
        b.Title,
        l.DueDate,
        CAST(julianday('now') - julianday(l.DueDate) AS INTEGER) AS DaysOverdue
    FROM Loans l
    JOIN BookCopies bc ON l.CopyID = bc.CopyID
    JOIN Books b ON bc.BookID = b.BookID
    WHERE l.ReturnDate IS NULL
      AND l.DueDate < date('now')
),
UserOverdue AS (
    SELECT
        UserID,
        COUNT(*) AS OverdueCount,
        MIN(DaysOverdue) AS MinDaysOverdue,
        MAX(DaysOverdue) AS MaxDaysOverdue,
        ROUND(AVG(DaysOverdue),1) AS AvgDaysOverdue,
        GROUP_CONCAT(Title, ' ; ') AS OverdueTitles
    FROM OverdueLoans
    GROUP BY UserID
),
UnpaidFines AS (
    SELECT
        UserID,
        COALESCE(SUM(Amount), 0.0) AS TotalUnpaidFines
    FROM Fines
    WHERE Paid = 0
    GROUP BY UserID
)
SELECT
    u.UserID,
    u.FirstName || ' ' || u.LastName AS UserName,
    COALESCE(uo.OverdueCount, 0) AS OverdueCount,
    COALESCE(uo.MinDaysOverdue, 0) AS MinDaysOverdue,
    COALESCE(uo.MaxDaysOverdue, 0) AS MaxDaysOverdue,
    COALESCE(uo.AvgDaysOverdue, 0.0) AS AvgDaysOverdue,
    COALESCE(up.TotalUnpaidFines, 0.0) AS TotalUnpaidFines,
    COALESCE(uo.OverdueTitles, '') AS OverdueTitles
FROM Users u
LEFT JOIN UserOverdue uo ON u.UserID = uo.UserID
LEFT JOIN UnpaidFines up ON u.UserID = up.UserID
WHERE COALESCE(uo.OverdueCount, 0) > 0
   OR COALESCE(up.TotalUnpaidFines, 0.0) > 0.0
ORDER BY OverdueCount DESC, TotalUnpaidFines DESC, MaxDaysOverdue DESC;
