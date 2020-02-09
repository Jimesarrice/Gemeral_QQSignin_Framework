using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace gemeral.robot.framework
{
    class sign
    {


        SQLiteConnection DBConnection;
        string dbstr = "C:\\QQSign.db";

        #region  SQLite访问类

        public void createNewDatabase(string filename)
        {
            SQLiteConnection.CreateFile(filename);
        }
        //创建一个连接到指定数据库
        public void connectToDatabase(string filename)
        {
            DBConnection = new SQLiteConnection("Data Source=" + filename + ";Version=3;");
            DBConnection.Open();
        }
        //在指定数据库中创建一个table
        public void CreateTable(string tablename, string data)
        {
            string sql = "create table " + tablename + " (" + data + ");";
            SQLiteCommand command = new SQLiteCommand(sql, DBConnection);
            command.ExecuteNonQuery();
        }
        //删除表记录
        public void dropTableSing(string tablename)
        {
            string sql = "DROP " + tablename + ";";
            SQLiteCommand command = new SQLiteCommand(sql, DBConnection);
            command.ExecuteNonQuery();
        }
        //插入一些数据
        public void fillTable(string tablename, string name, string data)
        {
            string sql = "insert into " + tablename + " (" + name + ") values (" + data + ");";
            SQLiteCommand command = new SQLiteCommand(sql, DBConnection);
            command.ExecuteNonQuery();
        }
        //使用sql查询语句，并显示结果
        public string printHighscores(string selecting, string tablename, string name, string data)
        {
            string re = null;
            string sql = "select " + selecting + " from " + tablename + " where " + name + "=\"" + data + "\";";
            SQLiteCommand command = new SQLiteCommand(sql, DBConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
                re = selecting + reader[selecting];
            Console.ReadLine();
            return re;
        }
        //执行SQL语句
        public string dosqlcom(string sql)
        {
            string re = null;
            SQLiteCommand command = new SQLiteCommand(sql, DBConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    re += reader[i] + ";";
                }
            Console.ReadLine();
            return re;
        }
        #endregion
        [DllImport("kernel32")]//返回0表示失败，非0为成功
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]//返回取得字符串缓冲区的长度
        private static extern long GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        public string IniReadValue(string section, string skey, string path)
        {
            StringBuilder temp = new StringBuilder(500);
            long i = GetPrivateProfileString(section, skey, "", temp, 500, path);
            return temp.ToString();
        }
        string FileName = "C:\\ERC.ini";
        string lv0 = "";
        string lv1 = "";
        string lv2 = "";
        string lv3 = "";
        string lv4 = "";
        string lv5 = "";
        string lv6 = "";
        string lv7 = "";

        public string maincllass(string singqq, string fromgroup, string com)
        {
            lv0 = IniReadValue("Setting", "lv0", FileName);
            lv1 = IniReadValue("Setting", "lv1", FileName);
            lv2 = IniReadValue("Setting", "lv2", FileName);
            lv3 = IniReadValue("Setting", "lv3", FileName);
            lv4 = IniReadValue("Setting", "lv4", FileName);
            lv5 = IniReadValue("Setting", "lv5", FileName);
            lv6 = IniReadValue("Setting", "lv6", FileName);
            lv7 = IniReadValue("Setting", "lv7", FileName);

            string result = "";
            try
            {
                connectToDatabase(dbstr);
                if (com == "sign")
                {
                    if (isselected(singqq, fromgroup))
                    {
                        result += singadd(singqq, fromgroup);
                    }
                    else
                    {
                        result += singcra(singqq, fromgroup);
                    }
                }
                else if (com == "mess")
                {
                    result += mymess(singqq, fromgroup);
                }
                else if (com == "sell")
                {
                    result += selectmem(singqq, fromgroup);
                }
                else if (com == "rp")
                {
                    if (isselected(singqq, fromgroup))
                    {
                        result += rpget(singqq);
                    }
                    else
                    {
                        result += "抱歉，没人认识你，发送‘签到’让大家认识一下你吧。。。";
                    }
                }
                else
                {
                    result += "?????????????";
                }
            }
            catch (Exception e)
            {
                result = "遭遇严重错误无法继续运行，请联系开发者获取帮助！";//+ e.ToString(); ;
            }
            return result;
        }

        private string rpget(string qq)
        {
            string result = "";
            int singdata = 0, money = 0, finish = 0;
            string sql = "select signdate,integral,finsing from signdata where signqq = '" + qq + "';";
            SQLiteCommand command = new SQLiteCommand(sql, DBConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                singdata = Convert.ToInt32(reader.GetValue(0));
                money = Convert.ToInt32(reader.GetValue(1));
                finish = Convert.ToInt32(reader.GetValue(2));
            }
            string day = DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString();
            if (finish.ToString() != day)
            {
                result += "今天还没有签到哦！请先‘签到’之后重新获取当前人品值哦~";
            }
            else
            {
                int rdi = ((money * finish) + 3141 + (singdata * finish)) % 101;
                string rpre = "";
                if (rdi <= 1)
                {
                    rpre = "哇~你真是喊人送煤----黑到家了。";
                }
                else if (rdi <= 10)
                {
                    rpre = "这运气没救了愿安好。";
                }
                else if (rdi <= 20)
                {
                    rpre = "心疼非洲来的孩子。";
                }
                else if (rdi <= 40)
                {
                    rpre = "相信自己你还有救。";
                }
                else if (rdi <= 70)
                {
                    rpre = "蓝天白云运气平平。";
                }
                else if (rdi <= 91)
                {
                    rpre = "紫气东来赶紧干活。";
                }
                else if (rdi <= 99)
                {
                    rpre = "啊~我被欧皇的欧气闪瞎了双眼。";
                }
                else if (rdi == 100)
                {
                    rpre = "同样是九年义务教育为何你如此优秀。";
                }
                else
                {
                    rpre = "你这个运气我不是很看得懂。";
                }
                result += "今天人品值为：" + rdi.ToString() + "\r\n运势评价：" + rpre;
            }

            DBConnection.Close();
            DBConnection.Dispose();
            GC.Collect();
            GC.WaitForPendingFinalizers();

            return result;

        }

        private string mymess(string qq, string group)
        {
            string res = "";
            int singdata = 0, money = 0, ddd = 0, integral = 0;
            string finish = "", le = "";
            string sql = "select signdate,money,finsing,integral from signdata where signqq = '" + qq + "';";
            SQLiteCommand command = new SQLiteCommand(sql, DBConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                singdata = Convert.ToInt32(reader.GetValue(0).ToString());
                money = Convert.ToInt32(reader.GetValue(1).ToString());
                finish = reader.GetValue(2).ToString();
                integral = Convert.ToInt32(reader.GetValue(3).ToString());
            }
            if (integral < 1000)
            {
                ddd = 1000 - integral;
                le = "当前等级" + lv0 + "，还需要签到" + ddd + "分升级";
            }
            else if (integral < 2000)
            {
                ddd = 2000 - integral;
                le = "当前等级" + lv1 + "，还需要签到" + ddd + "分升级";
            }
            else if (integral < 4000)
            {
                ddd = 4000 - integral;
                le = "当前等级" + lv2 + "，还需要签到" + ddd + "分升级";
            }
            else if (integral < 7000)
            {
                ddd = 7000 - integral;
                le = "当前等级" + lv3 + "，还需要签到" + ddd + "分升级";
            }
            else if (integral < 10000)
            {
                ddd = 10000 - integral;
                le = "当前等级" + lv4 + "，还需要签到" + ddd + "分升级";
            }
            else if (integral < 30000)
            {
                ddd = 30000 - integral;
                le = "当前等级" + lv5 + "，还需要签到" + ddd + "分升级";
            }
            else if (integral < 80000)
            {
                ddd = 80000 - integral;
                le = "当前等级" + lv6 + "，还需要签到" + ddd + "分升级";
            }
            else
            {
                le = "当前等级" + lv7 + "你已经此生无憾了。";
            }
            string day = DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString();
            if (finish == day)
            {
                res += "当前签到" + singdata + "天\r\n当前拥有金币" + money + "枚\r\n当前拥有积分" + integral + "\r\n今天已签到哦!\r\n" + le;
            }
            else
            {
                res += "当前签到" + singdata + "天\r\n当前拥有金币" + money + "枚\r\n当前拥有积分" + integral + "\r\n" + le;
            }

            return res;
        }

        private string selectmem(string qq, string group)
        {
            string res = "当前飙车榜如下：\r\n";
            string sql = "SELECT signqq,integral FROM signdata ORDER BY integral DESC LIMIT 5";
            int i = 0;
            SQLiteCommand command = new SQLiteCommand(sql, DBConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                res += "第" + (i + 1) + "名：" + reader.GetValue(0).ToString() + "，拥有积分：" + reader.GetValue(1).ToString() + "\r\n";
                i++;
            }
            return res;
        }

        private bool isselected(string singqq, string fromgroup)
        {
            bool a = false;
            string sql = "select * from signdata where signqq = '" + singqq + "';";
            SQLiteCommand command = new SQLiteCommand(sql, DBConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            if (reader.StepCount == 0)
            {
                a = false;
            }
            else
            {
                a = true;
            }
            return a;
        }

        private string singadd(string singqq, string fromgroup)
        {
            try
            {
                string result = "", le = "";

                Random rd = new Random();
                int rdi = rd.Next() % 70;
                int rda = rd.Next() % 30;
                string day = DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString();
                int singdata = 0, money = 0, ddd = 0, integral = 0, singnomber = 0;
                rdi = rdi + 30;
                string sql0 = "select signqq from signdata where finsing = '" + day + "';";
                SQLiteCommand command0 = new SQLiteCommand(sql0, DBConnection);
                SQLiteDataReader reader0 = command0.ExecuteReader();
                while (reader0.Read())
                {
                    for (int i = 0; i < reader0.FieldCount; i++)
                    {
                        singnomber++;
                    }
                }
                if (singnomber < 10)
                {
                    rdi += rda;
                }
                string sql = "select signdate,money,integral,finsing from signdata where signqq = '" + singqq + "';";
                SQLiteCommand command = new SQLiteCommand(sql, DBConnection);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    singdata = Convert.ToInt32(reader.GetValue(0).ToString());
                    money = Convert.ToInt32(reader.GetValue(1).ToString());
                    integral = Convert.ToInt32(reader.GetValue(2).ToString());
                    if (reader.GetValue(3).ToString() == day)
                    {
                        money -= rdi % 10;
                        result += "你今天已经签过到啦，重复签到惩罚金币减" + (rdi % 10) + "枚，下次注意喽！";
                    }
                    else
                    {
                        singdata++;
                        money += rdi;
                        integral += rdi;
                        if (integral < 1000)
                        {
                            ddd = 1000 - integral;
                            le = "当前等级" + lv0 + "，还需要签到" + ddd + "分升级";
                        }
                        else if (integral < 2000)
                        {
                            ddd = 2000 - integral;
                            le = "当前等级" + lv1 + "，还需要签到" + ddd + "分升级";
                        }
                        else if (integral < 4000)
                        {
                            ddd = 4000 - integral;
                            le = "当前等级" + lv2 + "，还需要签到" + ddd + "分升级";
                        }
                        else if (integral < 7000)
                        {
                            ddd = 7000 - integral;
                            le = "当前等级" + lv3 + "，还需要签到" + ddd + "分升级";
                        }
                        else if (integral < 10000)
                        {
                            ddd = 10000 - integral;
                            le = "当前等级" + lv4 + "，还需要签到" + ddd + "分升级";
                        }
                        else if (integral < 30000)
                        {
                            ddd = 30000 - integral;
                            le = "当前等级" + lv5 + "，还需要签到" + ddd + "分升级";
                        }
                        else if (integral < 80000)
                        {
                            ddd = 80000 - integral;
                            le = "当前等级" + lv6 + "，还需要签到" + ddd + "分升级";
                        }
                        else
                        {
                            le = "当前等级" + lv7 + "你已经此生无憾了。";
                        }
                        if (singnomber < 10)
                        {
                            result += "签到成功！\r\n本次获得金币：" + rdi + "枚\r\n你是今天第" + singnomber + "个签到的\r\n率先签到奖励" + rda + "枚金币\r\n累计签到：" + singdata + "天\r\n目前拥有金币：" + money + "枚\r\n目前拥有积分：" + integral + "\r\n" + le;
                        }
                        else
                        {
                            result += "签到成功！\r\n本次获得金币：" + rdi + "枚\r\n你是今天第" + singnomber + "个签到的\r\n累计签到：" + singdata + "天\r\n目前拥有金币：" + money + "枚\r\n目前拥有积分：" + integral + "\r\n" + le;
                        }
                    }
                }
                string sqlu = string.Empty;
                sqlu = @"UPDATE signdata SET signdate ='" + singdata + "',money = '" + money + "',integral = '" + integral + "',finsing = '" + day + "' where signqq = '" + singqq + "';";
                SQLiteCommand commandu = new SQLiteCommand(sqlu, DBConnection);
                commandu.ExecuteNonQuery();
                DBConnection.Close();
                DBConnection.Dispose();
                GC.Collect();
                GC.WaitForPendingFinalizers();
                return result;
            }
            catch
            {
                return "运行出错";
            }
        }

        private string singcra(string singqq, string fromgroup)
        {
            Random rd = new Random();
            int rdi = rd.Next() % 100 + 100;
            string day = DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString();
            int singdata = 1, money = rdi, integral = rdi;

            string sqlu = string.Empty;
            sqlu = @"INSERT INTO signdata VALUES('" + singqq + "','" + fromgroup + "','" + singdata + "','" + integral + "','" + day + "','" + money + "');";
            SQLiteCommand commandu = new SQLiteCommand(sqlu, DBConnection);
            commandu.ExecuteNonQuery();

            DBConnection.Close();
            DBConnection.Dispose();
            GC.Collect();
            GC.WaitForPendingFinalizers();

            return "首签成功！\r\n本次获得金币：" + rdi + "枚\r\n累计签到：" + singdata + "天\r\n目前拥有金币：" + money + "枚\r\n目前拥有积分：" + integral + "枚\r\n当前等级" + lv0 + "，还需要签到9天升级";

        }

    }

}
