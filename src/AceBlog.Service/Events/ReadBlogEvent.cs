using AceBlog.Entity;
using AceBlog.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AceBlog.Service.Events
{
    public class ReadBlogEvent : INotification
    {
        public int ReaderId { get; set; }
        public int BlogId { get; set; }
        public DateTime ReadTime { get; set; }
    }

    public class ReadBlogEventHandler : INotificationHandler<ReadBlogEvent>
    {
        IReadRecordRepository _readRecordRepository;
        IBlogRepository _blogRepository;
        public ReadBlogEventHandler(IReadRecordRepository readRecordRepository, IBlogRepository blogRepository)
        {
            this._readRecordRepository = readRecordRepository;
            this._blogRepository = blogRepository;
        }
        public Task Handle(ReadBlogEvent eventData, CancellationToken cancellationToken)
        {
            //判断是否已读，增加博客阅读数

            bool hasRead = this._readRecordRepository.HasRead(eventData.ReaderId, eventData.BlogId);
            if (hasRead)
                return Task.CompletedTask;

            this._readRecordRepository.Insert(new ReadRecord() { ReaderId = eventData.ReaderId, BlogId = eventData.BlogId, ReadTime = eventData.ReadTime });

            this._blogRepository.IncreaseReaderCount(eventData.BlogId);

            return Task.CompletedTask;
        }
    }
}
