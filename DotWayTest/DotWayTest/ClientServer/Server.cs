using System.Drawing;
using System.Collections.Generic;

namespace DotWayTest
{
    class Server
    {
        public readonly List<Client> clientList = new List<Client>();
        public Map map = null;

        public void GetMilkyMan(MilkyMan milkyMan)
        {
            this.map.milkyManList.Add(milkyMan);
        }

        public void SendMap(Map map)
        {
            foreach (Client client in this.clientList)
            {
                client.GetMap(map);
            }
        }

        public void GetConnect(Client client)
        {
            this.clientList.Add(client);
        }
    }
}
