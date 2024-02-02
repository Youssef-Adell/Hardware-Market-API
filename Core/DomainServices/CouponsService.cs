using AutoMapper;
using Core.DTOs.CouponDTOs;
using Core.Entities.OrderAggregate;
using Core.Exceptions;
using Core.Interfaces.IDomainServices;
using Core.Interfaces.IRepositories;

namespace Core.DomainServices;

public class CouponsService : ICouponsService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public CouponsService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    public async Task<IReadOnlyCollection<CouponDto>> GetCoupons()
    {
        var couponsEntities = await unitOfWork.Coupons.GetCoupons();

        var couponsDtos = mapper.Map<IReadOnlyCollection<Coupon>, IReadOnlyCollection<CouponDto>>(couponsEntities);

        return couponsDtos;
    }

    public async Task<CouponDto> GetCoupon(int id)
    {
        var couponEntity = await unitOfWork.Coupons.GetCoupon(id);

        if (couponEntity is null)
            throw new NotFoundException($"Coupon not found.");

        var couponDto = mapper.Map<Coupon?, CouponDto>(couponEntity);

        return couponDto;
    }

    public async Task<CouponDto> GetCoupon(string code)
    {
        var couponEntity = await unitOfWork.Coupons.GetCoupon(code);

        if (couponEntity is null)
            throw new NotFoundException($"Coupon not found.");

        var couponDto = mapper.Map<Coupon?, CouponDto>(couponEntity);

        return couponDto;
    }

    public async Task<int> AddCoupon(CouponForAddingDto couponToAdd)
    {
        var coupon = await unitOfWork.Coupons.GetCoupon(couponToAdd.Code);
        if (coupon != null)
            throw new UnprocessableEntityException("This id has been used by another coupon");

        var couponEntity = mapper.Map<CouponForAddingDto, Coupon>(couponToAdd);

        unitOfWork.Coupons.AddCoupon(couponEntity);

        await unitOfWork.SaveChanges();

        return couponEntity.Id;
    }

    public async Task UpdateCoupon(int id, CouponForUpdatingDto updatedCoupon)
    {
        var coupon = await unitOfWork.Coupons.GetCoupon(id);
        if (coupon is null)
            throw new NotFoundException($"Coupon not found.");

        var couponEntity = mapper.Map(updatedCoupon, coupon);

        unitOfWork.Coupons.UpdateCoupon(couponEntity);

        await unitOfWork.SaveChanges();
    }

    public async Task DeleteCoupon(int id)
    {
        var coupon = await unitOfWork.Coupons.GetCoupon(id);
        if (coupon is null)
            throw new NotFoundException($"Coupon not found.");

        unitOfWork.Coupons.DeleteCoupon(coupon);

        await unitOfWork.SaveChanges();
    }
}
