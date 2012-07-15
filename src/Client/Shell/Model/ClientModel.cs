using System.Collections.Generic;
using HDE.IpCamClientServer.Common.AspectOrientedFramework;

namespace HDE.IpCamClientServer.Client.Model
{
    public class ClientModel
    {
        #region Properties

        public List<ITool> Tools { get; private set; }

        #endregion

        #region Constructors

        public ClientModel()
        {
            Tools = new List<ITool>();
        }

        #endregion
    }
}
