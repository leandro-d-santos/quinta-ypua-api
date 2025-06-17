using Data.Context;
using Data.Financial.Entities;
using Data.Reservations.Entities;
using Domain.Common.Utils;
using Domain.Reservations.Models;
using Microsoft.EntityFrameworkCore;

namespace Domain.Financial.Repositories
{
    public sealed class FinancialRepository : IFinancialRepository
    {
        private readonly DbConnection context;

        public FinancialRepository(DbConnection context)
        {
            this.context = context;
        }

        public IList<Models.Financial> FindAll()
        {
            return context.Set<FinancialEntity>()
                .Select((financial) => financial.TransformToModel())
                .ToList();
        }

        public Models.Financial? FindById(Guid id)
        {
            Check.ThrowIfNull(id, nameof(id));
            FinancialEntity? reservation = FindFinancialEntityById(id);
            if (reservation is null)
            {
                return null;
            }
            return reservation.TransformToModel();
        }

        public void Create(Models.Financial financial)
        {
            Check.ThrowIfNull(financial, nameof(financial));
            FinancialEntity entity = FinancialEntity.CreateFromModel(financial);
            context.Set<FinancialEntity>().Add(entity);
        }

        public void Update(Models.Financial financial)
        {
            Check.ThrowIfNull(financial, nameof(financial));
            FinancialEntity? entity = FindFinancialEntityById(financial.Id);
            if (entity is null)
            {
                return;
            }
            entity.ReservationValue = financial.ReservationValue;
            entity.AdditionalValue = financial.AdditionalValue;
            entity.Payment = financial.Payment;
            context.Set<FinancialEntity>().Update(entity);
        }

        private FinancialEntity? FindFinancialEntityById(Guid id)
        {
            return context.Set<FinancialEntity>()
                .AsNoTracking()
                .SingleOrDefault(e => e.Id == id.ToString());
        }

        public void CheckOutAsync(Guid id)
        {
            Check.ThrowIfNull(id, nameof(id));
            FinancialEntity? entity = FindFinancialEntityById(id);
            if (entity is null)
            {
                return;
            }
            entity.Status = "Finalizado";
            context.Set<FinancialEntity>().Update(entity);
        }
    }
}