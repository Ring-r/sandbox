using System;

namespace DotWayTest
{
    class Client
    {
        public Server server = null;
        public Map map = null;

        public void SendMilkyMan(MilkyMan milkyMan)
        {
            this.server.GetMilkyMan(milkyMan);
        }

        public void GetMap(Map map)
        {
            this.map = map;
            this.map.milkyMan.Init();
            Options.State = Options.StateEnum.Choose;
        }

        public void SendConnect(Server server)
        {
            this.server = server;
            this.server.GetConnect(this);
        }
    }
}
