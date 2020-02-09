using Newbe.Mahua;
using Newbe.Mahua.MahuaEvents;
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;

namespace gemeral.robot.framework.MahuaEvents
{
    /// <summary>
    /// 群消息接收事件
    /// </summary>
    public class GroupMessageReceivedMahuaEvent
        : IGroupMessageReceivedMahuaEvent
    {
        private readonly IMahuaApi _mahuaApi;

        public GroupMessageReceivedMahuaEvent(
            IMahuaApi mahuaApi)
        {
            _mahuaApi = mahuaApi;
        }
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

        sign S = new sign();
        string FileName = "C:\\ERC.ini";
        string res = "null";
        public void ProcessGroupMessage(GroupMessageReceivedContext context)
        {
            // todo 填充处理逻辑
            //string cmstring = IniReadValue("Setting", "cmstring", FileName);
            string instring = IniReadValue("Setting", "instring", FileName);
            string restring = IniReadValue("Setting", "restring", FileName);
            //string[] cmarr;
            string[] inarr;
            string[] rearr;
            int numstr;
            //cmarr = cmstring.Split('|');
            inarr = instring.Split('|');
            rearr = restring.Split('|');
            if (inarr.Length == rearr.Length)
            {
                numstr = inarr.Length;
            }
            else
            {
                MessageBox.Show("配置文件出错,指令长度不匹配。");
                return;
            }
            // todo 填充处理逻辑
            //throw new NotImplementedException();
            if (context.Message == "签到" || context.Message == "飙车" || context.Message == "路过" || context.Message == "打卡")
            {
                try
                {
                    res = S.maincllass(context.FromQq, context.FromGroup, "sign");
                }
                catch
                {

                }
                _mahuaApi.SendGroupMessage(context.FromGroup)
                .At(context.FromQq)
                .Newline()
                .Text(res)
                .Newline()
                .Text("么么哒！")
                .Done();
            }
            else if (context.Message == "我的信息")
            {
                try
                {
                    res = S.maincllass(context.FromQq, context.FromGroup, "mess");
                }
                catch
                {

                }
                _mahuaApi.SendGroupMessage(context.FromGroup)
                .At(context.FromQq)
                .Newline()
                .Text(res)
                .Newline()
                .Text("么么哒！")
                .Done();
            }
            else if (context.Message == "飙车榜")
            {
                try
                {
                    res = S.maincllass(context.FromQq, context.FromGroup, "sell");
                }
                catch
                {

                }
                _mahuaApi.SendGroupMessage(context.FromGroup)
                .At(context.FromQq)
                .Newline()
                .Text(res)
                .Newline()
                .Text("么么哒！")
                .Done();
            }
            else if (context.Message == "人品")
            {
                try
                {
                    res = S.maincllass(context.FromQq, context.FromGroup, "rp");
                }
                catch
                {

                }
                _mahuaApi.SendGroupMessage(context.FromGroup)
                .At(context.FromQq)
                .Newline()
                .Text(res)
                .Newline()
                .Text("么么哒！")
                .Done();
            }
            else if (context.Message == "更新日志")
            {
                _mahuaApi.SendGroupMessage(context.FromGroup)
                .Newline()
                .Text("所以到底更新了什么呢？")
                .Done();
            }
            else
            {
                string thereturn = "";
                for (int i = 0; i < numstr; i++)
                {
                    if (context.Message == inarr[i])
                    {
                        thereturn = rearr[i];
                    }
                }
                if (thereturn != "")
                {
                    _mahuaApi.SendGroupMessage(context.FromGroup)
                    .Text(thereturn)
                    .Done();
                }
            }

            // 不要忘记在MahuaModule中注册
        }
    }
}
