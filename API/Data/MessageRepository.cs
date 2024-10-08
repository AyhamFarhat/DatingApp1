using API.DTOs;
using API.Entities;
using API.Helpers;
using API.interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class MessageRepository : IMessageRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public MessageRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void AddGroup(Group group)
        {
            _context.Groups.Add(group);
        }

        public void AddMessage(Message message)
        {
            _context.Messages.Add(message);
        }

        public void DeleteMessage(Message message)
        {
            _context.Messages.Remove(message);
        }

        public async Task<Connection> GetConnection(string connectionId)
        {
            return await _context.Connections.FindAsync(connectionId);
        }

        public async Task<Group> GetGroupForConnection(string connectionId)
        {
            return await _context.Groups
                .Include(x => x.Connections)
                .Where(x => x.Connections.Any(c => c.ConnectionId == connectionId))
                .FirstOrDefaultAsync();
        }

        public async Task<Message> GetMessage(int id)
        {
            return await _context.Messages.FindAsync(id);
        }

        public async Task<Group> GetMessageGroup(string groupName)
        {
            return await _context.Groups
                .Include(x => x.Connections)
                .FirstOrDefaultAsync(x => x.Name == groupName);
                
        }

        public async Task<PagedList<MessageDto>> GetMessagesForUser(MessageParams messageParams)
        {
            var query = _context.Messages
            .OrderByDescending(x => x.MessageSent)
            .AsQueryable();

            query = messageParams.Container switch {
                "Inbox" => query.Where(u => u.RecipientUsername == messageParams.Username && u.RecipientDeleted == false),
                "Outbox" => query.Where(u => u.SenderUsername == messageParams.Username && u.SenderDeleted ==false),
                _ => query.Where(u => u.RecipientUsername == messageParams.Username && u.DateRead == null && u.RecipientDeleted == false)
            };

            var messages = query.ProjectTo<MessageDto>(_mapper.ConfigurationProvider);
            return await PagedList<MessageDto>
                .CreateAsync(messages, messageParams.PageNumber, messageParams.PageSize);
        }

        public async Task<IEnumerable<MessageDto>> GetMessageThread(string currentUserUserName, string recipientUserName)
        {
            // //this code gives a large qery when i call this method, because of that i mad the below code to get more clean query in the API terminal
            // var messgaes = await _context.Messages
            //     .Include(u => u.Sender).ThenInclude(p => p.Photos)
            //     .Include(u => u.Recipient).ThenInclude(p => p.Photos)
            //     .Where(
            //         m => m.RecipientUsername == currentUserUserName && m.RecipientDeleted == false &&
            //         m.SenderUsername == recipientUserName ||
            //         m.RecipientUsername == recipientUserName && m.SenderDeleted == false &&
            //         m.SenderUsername == currentUserUserName
            //     )
            //     .OrderBy(m => m.MessageSent)
            //     .ToListAsync();
            // var unreadMessages = messgaes.Where(m => m.DateRead == null &&
            //     m.RecipientUsername == currentUserUserName).ToList();
            
            // if(unreadMessages.Any()){
            //     foreach (var message in unreadMessages){
            //         message.DateRead = DateTime.UtcNow;
            //     }

            //     //await _context.SaveChangesAsync();
            //     // Note: Removed direct call to context.SaveChangesAsync() to ensure changes
            //     // are saved via the Unit of Work's Complete method, maintaining proper use of
            //     // the Unit of Work pattern and avoiding potential issues.
            // }
            // return _mapper.Map<IEnumerable<MessageDto>>(messgaes);


             var query = _context.Messages
                                .Where(
                    m => m.RecipientUsername == currentUserUserName && m.RecipientDeleted == false &&
                    m.SenderUsername == recipientUserName ||
                    m.RecipientUsername == recipientUserName && m.SenderDeleted == false &&
                    m.SenderUsername == currentUserUserName
                )
                .OrderBy(m => m.MessageSent)
                .AsQueryable();

            var unreadMessages = query.Where(m => m.DateRead == null &&
                m.RecipientUsername == currentUserUserName).ToList();
            
            if(unreadMessages.Any()){
                foreach (var message in unreadMessages){
                    message.DateRead = DateTime.UtcNow;
                }

                //await _context.SaveChangesAsync();
                // Note: Removed direct call to context.SaveChangesAsync() to ensure changes
                // are saved via the Unit of Work's Complete method, maintaining proper use of
                // the Unit of Work pattern and avoiding potential issues.
            }
            return await query.ProjectTo<MessageDto>(_mapper.ConfigurationProvider).ToListAsync();

            
        }

        public void RemoveConnection(Connection connection)
        {
            _context.Connections.Remove(connection);
        }

        // public async Task<bool> SaveAllAsync()
        // {
        //     return await _context.SaveChangesAsync() > 0;
        // }
    }
}