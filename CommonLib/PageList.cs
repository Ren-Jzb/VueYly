using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib
{
    public class PageList
    {

        //地址：https://www.cnblogs.com/lunawzh/p/7632926.html
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="curPage">当前页</param>
        /// <param name="total">记录数</param>
        /// <param name="pagesize">每页显示的记录数</param>
        /// <param name="showPageNum">页码显示数量</param>
        public PageList(int curPage, int total, int pagesize = 2, int showPageNum = 9)
        {
            this.total = total;
            this.pagesize = pagesize;
            this.curPage = curPage;
            this.showPageNum = showPageNum;
            this.firstNum = 1;
            this.lastNum = this.totalPages;
            this.pagelist = this.getPagelist();
        }
        //前面的点，前面省略的页数用.来代表，
        public bool previousSpot { get; set; }
        //后面的点，后面省略的页数用.来代表，
        public bool nextSpot { get; set; }
        //第一个页数，一般都是1
        public int firstNum { get; set; }
        //最后一个页数,也就是最大页数
        public int lastNum { get; set; }
        //默认页面显示最多页号数目
        public int showPageNum { get; set; }
        public int total { get; set; }//总记录数
        //总页数
        public int totalPages
        {
            get
            {
                return (int)Math.Ceiling((double)total / pagesize);
            }
        }
        public int curPage { get; set; }//当前页
        public int pagesize { get; set; }//每页大小

        //页数列表，此列表中不包含第一页和最后一页
        public List<int> pagelist { get; set; }

        public List<int> getPagelist()
        {
            var p = new List<int>();
            if (totalPages <= showPageNum)//全部显示
            {
                for (int i = 2; i < totalPages; i++)
                {
                    p.Add(i);
                }
            }
            else
            {
                var yiban = ((int)((showPageNum + 1) / 2)) - 1;//前后保留页数大小
                if (curPage - yiban > 1 && curPage + yiban < totalPages)
                {
                    //两头都可取值
                    this.previousSpot = this.nextSpot = true;
                    for (int i = curPage - yiban + 1; i < curPage + yiban; i++)
                    {
                        p.Add(i);
                    }
                }
                else if (curPage - yiban > 1)
                {
                    //右头值少
                    this.previousSpot = true;
                    for (int i = totalPages - 1; i > totalPages - showPageNum + 2; i--)
                    {
                        p.Add(i);
                    }

                }
                else if (curPage - yiban <= 1)
                {
                    //左头值少
                    this.nextSpot = true;
                    for (int i = 2; i < showPageNum; i++)
                    {
                        p.Add(i);
                    }
                }
            }
            return p.OrderBy(x => x).ToList();
        }
    }
}
