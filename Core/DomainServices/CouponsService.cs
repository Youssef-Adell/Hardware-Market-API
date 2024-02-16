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

    public async Task<IReadOnlyCollection<CouponResponse>> GetCoupons()
    {
        var couponsEntities = await unitOfWork.Coupons.GetCoupons();

        var couponsDtos = mapper.Map<IReadOnlyCollection<Coupon>, IReadOnlyCollection<CouponResponse>>(couponsEntities);

        return couponsDtos;
    }

    public async Task<CouponResponse> GetCoupon(Guid id)
    {
        var couponEntity = await unitOfWork.Coupons.GetCoupon(id);

        if (couponEntity is null)
            throw new NotFoundException($"Coupon not found.");

        var couponDto = mapper.Map<Coupon?, CouponResponse>(couponEntity);

        return couponDto;
    }

    public async Task<CouponResponse> GetCoupon(string code)
    {
        var couponEntity = await unitOfWork.Coupons.GetCoupon(code);

        if (couponEntity is null)
            throw new NotFoundException($"Coupon not found.");

        var couponDto = mapper.Map<Coupon?, CouponResponse>(couponEntity);

        return couponDto;
    }

    public async Task<CouponResponse> AddCoupon(CouponAddRequest couponAddRequest)
    {
        var couponExists = await unitOfWork.Coupons.CouponExists(couponAddRequest.Code);
        if (couponExists)
            throw new ConfilctException($"The coupon code {couponAddRequest.Code} is already in use by another coupon.");

        var couponEntity = mapper.Map<CouponAddRequest, Coupon>(couponAddRequest);

        unitOfWork.Coupons.AddCoupon(couponEntity);

        await unitOfWork.SaveChanges();

        var couponDto = mapper.Map<Coupon, CouponResponse>(couponEntity);

        return couponDto;
    }

    public async Task<CouponResponse> UpdateCoupon(Guid id, CouponUpdateRequest couponUpdateRequest)
    {
        var coupon = await unitOfWork.Coupons.GetCoupon(id);
        if (coupon is null)
            throw new NotFoundException($"Coupon not found.");

        var couponEntity = mapper.Map(couponUpdateRequest, coupon);

        unitOfWork.Coupons.UpdateCoupon(couponEntity);

        var couponDto = mapper.Map<Coupon, CouponResponse>(couponEntity);

        return couponDto;
    }

    public async Task DeleteCoupon(Guid id)
    {
        var coupon = await unitOfWork.Coupons.GetCoupon(id);
        if (coupon is null)
            throw new NotFoundException($"Coupon not found.");

        unitOfWork.Coupons.DeleteCoupon(coupon);

        await unitOfWork.SaveChanges();
    }

    public async Task<double> CalculateCouponDiscount(double subtotalAmount, string couponCode)
    {
        var coupon = await unitOfWork.Coupons.GetCoupon(couponCode);

        if (coupon is null || !coupon.IsValid || subtotalAmount < coupon.MinPurchaseAmount)
            return 0;

        var discount = subtotalAmount * (coupon.DiscountPercentage / 100); //we are applying the discount on subtotal not total

        var finalDiscount = (discount <= coupon.MaxDiscountAmount) ? discount : coupon.MaxDiscountAmount;

        return finalDiscount;
    }

}
