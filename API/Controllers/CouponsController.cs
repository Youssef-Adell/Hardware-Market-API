using System.ComponentModel.DataAnnotations;
using Core.DTOs.CouponDTOs;
using Core.Interfaces.IDomainServices;
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

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetCoupon(int id)
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
    public async Task<IActionResult> AddCoupon(CouponForAddingDto couponToAdd)
    {
        var couponId = await brandsService.AddCoupon(couponToAdd);

        return CreatedAtAction(nameof(GetCoupon), new { id = couponId }, null);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateCoupon(int id, CouponForUpdatingDto updatedCoupon)
    {
        await brandsService.UpdateCoupon(id, updatedCoupon);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteCoupon(int id)
    {
        await brandsService.DeleteCoupon(id);

        return NoContent();
    }
}