using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Dapper;
using Oracle.ManagedDataAccess.Client;

namespace App2
{
    class TestClass1 : ITest
    {
        public void Test()
        {
            using (var con = Config.GetConnection())
            {
                con.Open();
                Console.ReadKey();
            }
        }
    }

    class TestClass2 : ITest
    {
        public void Test()
        {
            using (var con = Config.GetConnection())
            {
                //var count = con.QueryFirst<int>("SELECT COUNT(1) FROM APPL.ORDER_HEADERS_ALL");
                var count = con.ExecuteScalar<dynamic>("SELECT COUNT(1) FROM APPL.ORDER_HEADERS_ALL");
                Console.WriteLine(count);
            }
        }
    }

    class TestClass3 : ITest
    {
        public void Test()
        {
            using (var con = Config.GetConnection())
            {
                var orders = con.Query<TaskOrder>(
                    "SELECT HEADER_ID, WO_NUMBER, ORDER_NUMBER, REQUEST_DATE,JHRQ FROM APPL.ORDER_HEADERS_ALL WHERE SPLIT_CREATED_DATE > SYSDATE - 30");
                Console.WriteLine(orders.Count());

                foreach (var order in orders)
                {
                    Console.WriteLine(string.Format("{0}, {1}, {2}, {3}", order.WO_NUMBER, order.ORDER_NUMBER, order.REQUEST_DATE, order.JHRQ));
                }
            }
        }
    }

    //好像只能处理一行数据，若是一对多的关系还得使用multiple query results来处理
    class TestClass4 : ITest
    {
        public void Test()
        {
            var sql = @"SELECT T1.HEADER_ID,
                               T1.ORDER_NUMBER,
                               T1.WO_NUMBER,
                               T1.REQUEST_DATE,
                               T1.JHRQ,
                               T2.LINE_ID,
                               T2.INVENTORY_ITEM_ID,
                               T2.ORDERED_QUANTITY,
                               T2.QTY
                          FROM ORDER_HEADERS_ALL T1, ORDER_LINES_ALL T2
                         WHERE T1.HEADER_ID = T2.HEADER_ID
                           AND T1.WO_NUMBER = T2.WO_NUMBER
                           AND T1.WO_NUMBER = 1000090720001 AND ROWNUM = 1";

            using (var con = Config.GetConnection())
            {
                var data = con.Query<TaskOrder, TaskOrderDetails, TaskOrder>(
                    sql, (taskOrder, orderLines) => { taskOrder.LINE = orderLines; return taskOrder; }, splitOn: "HEADER_ID, WO_NUMBER");

                var order = data.First();

                Console.WriteLine(order.HEADER_ID + ", " + order.LINE.LINE_ID);

                //foreach (var line in order.LINES)
                //{
                //    Console.WriteLine(string.Format("{0}, {1}, {2}, {3}", line.WO_NUMBER, line.LINE_ID, line.ORDERED_QUANTITY, line.QTY));
                //}
            }
        }
    }

    //not work in oracle
    class TestClass5 : ITest
    {
        public void Test()
        {
            var sql = @"SELECT T1.HEADER_ID,
                               T1.ORDER_NUMBER,
                               T1.WO_NUMBER,
                               T1.REQUEST_DATE,
                               T1.JHRQ 
                          FROM ORDER_HEADERS_ALL T1
                         WHERE T1.WO_NUMBER = 1000090720001;

                        SELECT T2.LINE_ID,
                               T2.INVENTORY_ITEM_ID,
                               T2.ORDERED_QUANTITY,
                               T2.QTY
                          FROM ORDER_LINES_ALL T2
                         WHERE T2.WO_NUMBER = 1000090720001";

            using (var con = Config.GetConnection())
            {
                using (var multi = con.QueryMultiple(sql))
                {
                    var order = multi.Read<TaskOrder>().Single();
                    var lines = multi.Read<TaskOrderDetails>().ToList();

                    Console.WriteLine(order.HEADER_ID);
                    Console.WriteLine(lines.Count);
                }
            }
        }
    }

    //multiple query results in oracle
    class TestClass6 : ITest
    {
        public void Test()
        {
            var sql = @"BEGIN 
                OPEN :RCS1 FOR SELECT T1.HEADER_ID,
                               T1.ORDER_NUMBER,
                               T1.WO_NUMBER,
                               T1.REQUEST_DATE,
                               T1.JHRQ 
                          FROM ORDER_HEADERS_ALL T1
                         WHERE T1.WO_NUMBER = 1000090720001;

                OPEN :RCS2 FOR SELECT T2.LINE_ID,
                               T2.INVENTORY_ITEM_ID,
                               T2.ORDERED_QUANTITY,
                               T2.QTY
                          FROM ORDER_LINES_ALL T2
                         WHERE T2.WO_NUMBER = 1000090720001";

            var paras = new OracleDynamicParameters();
            paras.Add(":RCS1", OracleDbType.RefCursor, ParameterDirection.Output);
            paras.Add(":RCS2", OracleDbType.RefCursor, ParameterDirection.Output);

            using (var con = Config.GetConnection())
            {
                using (var multi = con.QueryMultiple(sql,paras))
                {
                    var order = multi.Read<TaskOrder>().Single();
                    var lines = multi.Read<TaskOrderDetails>().ToList();

                    Console.WriteLine(order.HEADER_ID);
                    Console.WriteLine(lines.Count);
                }
            }
        }
    }



    class TaskOrder
    {
        public ulong HEADER_ID { get; set; }

        public ulong WO_NUMBER { get; set; }

        public ulong ORDER_NUMBER { get; set; }

        public DateTime? REQUEST_DATE { get; set; }

        public DateTime? JHRQ { get; set; }

        public TaskOrderDetails LINE { get; set; }

        //public List<TaskOrderDetails> LINES { get; set; }
    }

    class TaskOrderDetails
    {
        public ulong HEADER_ID { get; set; }

        public ulong LINE_ID { get; set; }

        public ulong WO_NUMBER { get; set; }

        public ulong INVENTORY_ITEM_ID { get; set; }

        public int ORDERED_QUANTITY { get; set; }

        public int QTY { get; set; }
    }
}
