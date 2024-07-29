using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEC2FHIR
{
    /// <summary>
    /// 資源處理行為
    /// </summary>
    public enum ResourceAction
    {
        None,
        /// <summary>使用既有</summary>
        Use, 
        /// <summary>新增</summary>
        Create,
        /// <summary>使用既有並且更新</summary>
        Update,
    }
}
