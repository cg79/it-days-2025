
```aiignore
[{
	tableName: "Orders",
	records: 100,
	columnValues: {
		userId: {
			value:{
				type: "table"
				column: "Users.Id",
			}
		},
		amount:{
			value: {
				type: "random",
				min: 0,
				max: 1000
			}
		}
	}
}]
```