using Microsoft.AspNetCore.SignalR;
using SignalRServer.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRServer.Business
{
    public class MyBusiness
    {
        readonly IHubContext<MyHub> _hubContext;

        public MyBusiness(IHubContext<MyHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendMessageAsync(string userName, string message)
        {

            #region NOT!!!
            /* gelen mesaj hub ile karşılanıyor bu grubu tercih eden fonksiyona
                 * subscribe olan diğer kullanıcıların her biri gelen mesajı anında alıyor 
                 * Clients.All.SendAsync() -> subscribe olan tüm kullanıcılara mesajı gönder
                 * ("receiveMessage", message) -> receiveMessage isimli clientta bir fonksiyon bekliyorum
                 *      o fonksiyonu tetikle, tetiklerken de clientın gönderdiği mesajı bunlara gönder
                 *  1- sisteme signalR(web socket) teknolojisini kullanacağını bildirmem lazım  
                 *  2- Bu hub'ı hangi endpointte kullanacağını da bildirmemiz gerekiyor
                 */
            #endregion

            //ConnectClient();
            await _hubContext.Clients.All.SendAsync("pullMessage", "fg", "Topics");
        }
    }
}

// bir kontorller üzerinden istek üzerinde myHuba baglı olan tum clientlara iletiler gönderebiliriz