using System;
using System.Linq;
using ef_base_repository;
using ef_gpt;
using ef_implementation_tests;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OpenAI.Chat;
using TextToSqlDemo.Models;
using TextToSqlDemo.Services;

// youtube: https://www.youtube.com/watch?v=e1wEgEH825A
namespace TextToSqlDemo
{
    class Program 
    {
        static async Task Main(string[] args)
        {
            //V1 prog = new V1();
            V2 prog = new V2();
            await prog.Run();
        }
    }
}