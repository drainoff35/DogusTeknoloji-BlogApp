using AutoMapper;
using DogusTeknoloji_BlogApp.Services.DTOs.PostDtos;
using DogusTeknoloji_BlogApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

public class HomeController : Controller
{
    private readonly IPostService _postService;
    private readonly ICategoryService _categoryService;
    private readonly IMapper _mapper;

    public HomeController(IPostService postService, ICategoryService categoryService, IMapper mapper)
    {
        _postService = postService;
        _categoryService = categoryService;
        _mapper = mapper;
    }

    public async Task<IActionResult> Index(int? categoryId = null)
    {
        ViewBag.Categories = await _categoryService.GetAllAsync();

        var posts = categoryId.HasValue
            ? await _postService.GetPostsByCategoryIdAsync(categoryId.Value)
            : await _postService.GetAllWithCategoriesAsync();

        var postDtos = _mapper.Map<List<PostDetailDto>>(posts);
        return View(postDtos);
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View();
    }
}
