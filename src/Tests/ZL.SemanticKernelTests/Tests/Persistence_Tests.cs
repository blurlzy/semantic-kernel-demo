﻿using ZL.SemanticKernelDemo.Host.Models;
using ZL.SemanticKernelDemo.Host.Persistence;

namespace ZL.SemanticKernelTests.Tests
{
    public class Persistence_Tests
    {
        // cosmos settings
        private readonly string _cosmosConnection = SecretManager.GetSecret(SecretKeys.CosmosConnection);
        private readonly string _cosmosDb = SecretManager.GetSecret(SecretKeys.CosmosDb);
        private readonly string _chatSessionContainer = SecretManager.GetSecret(SecretKeys.ChatSessionContainer);
        private readonly string _chatMessageContainer = SecretManager.GetSecret(SecretKeys.ChatHistoryContainer);


        private readonly CosmosDbContext<ChatSession> _chatSessionContext;
        private readonly CosmosDbChatMessageContext _chatMessageContext;

        private readonly ChatSessionRepository _chatSessionRepository;
        private readonly ChatMessageRepository _chatMessageRepository;

        // ctor
        public Persistence_Tests()
        {
            // cosmos db context
            _chatSessionContext = new CosmosDbContext<ChatSession>(_cosmosConnection, _cosmosDb, _chatSessionContainer);
            _chatMessageContext = new CosmosDbChatMessageContext(_cosmosConnection, _cosmosDb, _chatMessageContainer);

            // repos
            _chatSessionRepository = new ChatSessionRepository(_chatSessionContext);
            _chatMessageRepository = new ChatMessageRepository(_chatMessageContext);

        }

        [Theory]
        [InlineData("New Chat 3", "zongyi")]
        public async Task Add_ChatSession_Test(string title, string userId)
        {
            // 
            var newChatSession = new ChatSession(title, userId, userId);

            // save
            await _chatSessionRepository.CreateAsync(newChatSession);
        }

        [Theory]
        [InlineData("zongyi")]
        public async Task List_ChatSessions_Test(string userId)
        {
            var entities = await _chatSessionRepository.ListChatSessionsAsync(userId);

            Assert.NotNull(entities);
        }


        [Theory]
        [InlineData("fc7c961b-5913-4036-a137-941ee2daf1c0")]
        public async Task SoftDelete_ChatSession_Test(string id)
        {
            await _chatSessionRepository.SoftDeleteAsync(id);
        }

        [Theory]
        [InlineData("fc7c961b-5913-4036-a137-941ee2daf1c0")]
        [InlineData("c88fb17b-e2aa-4113-b0fc-31eacbc71807")]
        [InlineData("575e282d-0484-41e1-968f-822edde7c276")]
        [InlineData("9860ae55-3757-4bf1-b755-b23c498b150f")]
        [InlineData("769aeea7-41eb-48bf-a338-4a1ae832f4ff")]
        [InlineData("8b80147a-fbd8-4a12-adce-1d7eac50753d")]
        public async Task Delete_ChatSession_Test(string id)
        {
            await _chatSessionRepository.DeleteAsync(id);
        }

        [Theory]
        [InlineData("chatId")]
        public async Task List_Chat_History_Test(string chatId)
        {
            var chatMessages = await _chatMessageRepository.GetHistoryMessagesAsync(chatId, 0, 10);

            Assert.NotNull(chatMessages);
        }
    }
}
