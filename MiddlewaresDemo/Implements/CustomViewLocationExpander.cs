using Microsoft.AspNetCore.Mvc.Razor;

namespace MiddlewaresDemo.Implements
{

    /// <summary>
    /// 扩展了视图位置，以便查找特定视图名称，同时保留原始视图位置。
    /// 在 ExpandViewLocations 方法中，我们首先遍历传入的视图位置，并将其添加到结果集中。
    /// 然后，我们检查视图位置是否包含“/Views/”字符串。
    /// 如果是，我们替换它为包含视图名称的格式化字符串。
    /// 最后，我们返回修改后的结果集。
    /// </summary>
    public class CustomViewLocationExpander : IViewLocationExpander
    {
        private const string _viewLocationFormat = "/Views/{0}.cshtml";


        /// <summary>
        /// PopulateValues 方法是在实例化视图引擎前调用的方法，它可以在这里传递任何需要的上下文数据。
        /// </summary>
        /// <param name="context"></param>
        public void PopulateValues(ViewLocationExpanderContext context)
        {
            //不是必需要实现
        }


        /// <summary>
        /// ExpandViewLocations 方法是在视图引擎搜索视图位置时调用的方法。
        /// 它接收 ViewLocationExpanderContext 对象和一组视图位置，
        /// 然后返回一个经过修改的视图位置集合。
        /// </summary>
        /// <param name="context"></param>
        /// <param name="viewLocations"></param>
        /// <returns></returns>
        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            var result = new List<string>();

            foreach (var location in viewLocations)
            {
                result.Add(location);
                if (!string.IsNullOrEmpty(location) && location.Contains("/Views/"))
                {
                    var viewName = context.ViewName;
                    result.Add(location.Replace("/Views/", string.Format(_viewLocationFormat, viewName)));
                }
            }

            return result;
        }
    }
}
