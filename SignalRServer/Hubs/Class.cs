

//namespace SignalRServer.Hubs
//{
//    public class MyHub : Hub
//    {


//        static List<string> clients = new List<string>();
//        /* gelen serveri hub'ın karşılayabilmesi için parametre kullanıyoruz
//         * clientın gönderdiği data hangisiyse burada parametre olarak gözükecek */

//        public async Task SendMessageAsync(string userName, string message)
//        {

//            #region NOT!!!
//            /* gelen mesaj hub ile karşılanıyor bu grubu tercih eden fonksiyona
//                 * subscribe olan diğer kullanıcıların her biri gelen mesajı anında alıyor 
//                 * Clients.All.SendAsync() -> subscribe olan tüm kullanıcılara mesajı gönder
//                 * ("receiveMessage", message) -> receiveMessage isimli clientta bir fonksiyon bekliyorum
//                 *      o fonksiyonu tetikle, tetiklerken de clientın gönderdiği mesajı bunlara gönder
//                 *  1- sisteme signalR(web socket) teknolojisini kullanacağını bildirmem lazım  
//                 *  2- Bu hub'ı hangi endpointte kullanacağını da bildirmemiz gerekiyor
//                 */
//            #endregion

//            ConnectClient();
//            //await Clients.User().SendAsync().
//            await Clients.All.SendAsync("pullMessage", topic, "Topics");
//            //await Clients.Group


//        }
//        // sistemde bir bağlantı varken bu fonksiyon gerçekleşecek

//        public override async Task OnConnectedAsync()
//        {
//            // connect oldugunda clientlara connectinId 'yi ekle
//            // ekledik ama clientların bundan haberi yok
//            // bu fonksiyonu dinleyenler mevcudiyetteki tüm clientları ekleyebilecekler
//            clients.Add(Context.ConnectionId);
//            await Clients.All.SendAsync("clients", clients);
//            await Clients.All.SendAsync("userJoined", Context.ConnectionId);
//        }
//        public override async Task OnDisconnectedAsync(Exception exception)
//        {
//            clients.Remove(Context.ConnectionId);
//            await Clients.All.SendAsync("clients", clients);
//            await Clients.All.SendAsync("userLeaved", Context.ConnectionId);
//        }

//    }
//}



/* sunucuda ki hub'a client'ı bağlamam lazım
 * bu client hangi huba yani server'a bağlanacağını bildirmek gerekiyor
 * ------------
 * bağlantıyı kullananan tüm clientlara mesaj göndermek istiyorsam;
 * tek yolu elimdeki hub'ın bunu sağlaması lazım
 * elimdeki hub diyorki bana gelen mesajı bana bağlı olan tüm clientlara gönderirim
 * dolayısıyla sen hangi clienttaysan o clienttan ilgili hub'a herhangi bir mesaj
 * gönderdiğinde o diğer clientlara gidicektir
 */
