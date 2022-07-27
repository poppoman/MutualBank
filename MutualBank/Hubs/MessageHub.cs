using Microsoft.AspNetCore.SignalR;
using MutualBank.Models;
namespace MutualBank.Hubs
{
    public class MessageHub:Hub
    {
        private MutualBankContext _mutualBankContext;
        public MessageHub(MutualBankContext mutualBankContext)
        {
            _mutualBankContext = mutualBankContext;
        }

        public async Task Online(string user)
        {
            var UserId = _mutualBankContext.Logins.Where(x => x.LoginName == user).Select(x=>x.LoginId).FirstOrDefault();
            var Msg = _mutualBankContext.Messages.Where(x => x.MsgToUserId == UserId && x.MsgIsRead == false).Count();
            await Clients.Client(Context.ConnectionId).SendAsync("UserOnline", user, UserId, Msg);
        }
    }
}
