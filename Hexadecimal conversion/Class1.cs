using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Wox.Plugin;

namespace Hexadecimal_conversion
{
    public class Main : IPlugin
    {
        public void Init(PluginInitContext context) { }
        public List<Result> Query(Query query)
        {
            List<Result> results = new List<Result>();
            List<Int16> tf = new List<Int16>(4);
            tf.Add(10);
            tf.Add(16);
            tf.Add(2);
            tf.Add(8);
            int flag = 0;

            switch (query.Search.Substring(0, 2))
            {
                case "b~":
                    tf.Remove(2);
                    flag = 2;
                    break;
                case "o~":
                    tf.Remove(8);
                    flag = 8;
                    break;
                case "d~":
                    tf.Remove(10);
                    flag = 10;
                    break;
                case "x~":
                    tf.Remove(16);
                    flag = 16;
                    break;
                default:
                    break;
            }

            foreach (var s in tf)
            {
                string result = ConvertString(query.Search.Substring(2), flag, s);
                if (result == null)//不合法的输入
                    continue;
                //向list里添加数据
                results.Add(new Result()
                {
                    Title = "   " + flag + "->" + s + "：    " + result,
                    SubTitle = "    回车复制结果",
                    IcoPath = "img/zhuanhuan.png",  //相对于插件目录的相对路径
                    Action = e =>
                    {
                        // 处理用户选择之后的操作
                        Clipboard.SetText(result);
                        //返回false告诉Wox不要隐藏查询窗体，返回true则会自动隐藏Wox查询窗口
                        return true;
                    }
                });
            }

            return results;
        }

        //进行转换
        private string ConvertString(string value, int frombase, int tobase)
        {
            string s;
            int intvalue;
            try
            {
                intvalue = Convert.ToInt32(value, frombase);
                s = Convert.ToString(intvalue, tobase);
            }
            catch(ArgumentException)
            {
                return null;
            }
            catch (FormatException)
            {
                return null;
            }
            catch (OverflowException)
            {
                return null;
            }
            return s;
        }
    }
}