using Core.DTOs.CouponDTOs;
using Core.Interfaces.IDomainServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/coupons")]
public class CouponsController : ControllerBase
{
    private readonly ICouponsService brandsService;

    public CouponsController(ICouponsService brandsService)
    {
        this.brandsService = brandsService;
    }

    [HttpGet]
    public async Task<IActionResult> GetCoupons()
    {
        var result = await brandsService.GetCoupons();

        return Ok(result);
    }

    [HttpGet("{id:Guid}")]
    public async Task<IActionResult> GetCoupon(Guid id)
    {
        var result = await brandsService.GetCoupon(id);

        return Ok(result);
    }

    [HttpGet("{code}")]
    public async Task<IActionResult> GetCoupon(string code)
    {
        var result = await brandsService.GetCoupon(code);

        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddCoupon(CouponAddRequest couponAddRequest)
    {
        var createdCoupon = await brandsService.AddCoupon(couponAddRequest);

        return CreatedAtAction(nameof(GetCoupon), new { id = createdCoupon.Id }, createdCoupon);
    }

    [HttpPut("{id:Guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateCoupon(Guid id, CouponUpdateRequest couponUpdateRequest)
    {
        var updatedCoupon = await brandsService.UpdateCoupon(id, couponUpdateRequest);

        return Ok(updatedCoupon);
    }

    [HttpDelete("{id:Guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteCoupon(Guid id)
    {
        await brandsService.DeleteCoupon(id);

        return NoContent();
    }
}