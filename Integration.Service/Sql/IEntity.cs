using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration.Service.Sql
{
    /// <summary>
    /// 实体
    /// </summary>
    public interface IEntity
    {
        long Id { get; set; }
    }

    /// <summary>
    /// 审计实体
    /// </summary>
    public interface IAuditEntity : IEntity
    {
        long CreateBy { get; set; }

        string CreateName { get; set; }

        DateTime CreateTime { get; set; }

        long? LastUpdateBy { get; set; }

        string LastUpdateName { get; set; }

        DateTime? LastUpdateTime { get; set; }
    }

    /// <summary>
    /// 软删除实体
    /// </summary>
    public interface ISoftDelete : IEntity
    {
        /// <summary>
        /// 软删除
        /// </summary>
        bool IsDeleted { get; set; }
    }

    /// <summary>
    /// 租户数据
    /// </summary>
    public interface ITenantEntity
    {
        /// <summary>
        /// 租户id
        /// </summary>
        long TenantId { get; set; }
    }


    public interface IFullEntity : IEntity, IAuditEntity, ISoftDelete
    {

    }

    public abstract class Entity : IEntity
    {
        public long Id { get; set; }
    }

    public abstract class AuditEntity : Entity, IAuditEntity
    {
        public long CreateBy { get; set; }

        public string CreateName { get; set; }

        public DateTime CreateTime { get; set; }

        public long? LastUpdateBy { get; set; }

        public string LastUpdateName { get; set; }

        public DateTime? LastUpdateTime { get; set; }
    }

    public abstract class FullEntity : AuditEntity, ISoftDelete
    {
        public bool IsDeleted { get; set; }
    }
}
