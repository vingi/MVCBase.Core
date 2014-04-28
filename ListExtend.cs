using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVCBase.Core
{
    public class ListExtend
    {
        #region 对List进行随机排序
        /// <summary>
        /// 对List进行随机排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ListT"></param>
        /// <returns></returns>
        public static List<T> RandomSortList<T>(List<T> ListT)
        {
            Random random = new Random();
            List<T> newList = new List<T>();
            foreach (T item in ListT)
            {
                newList.Insert(random.Next(newList.Count + 1), item);
            }
            return newList;
        }
        #endregion
    }
}
