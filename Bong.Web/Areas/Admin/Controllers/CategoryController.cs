using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

using Bong.Web.Areas.Admin.Models;
using Bong.Web.Areas.Admin.DataSource;

using Bong.Core.Domain.Goods;
using Bong.Services.Goods;


namespace Bong.Web.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        #region Fields

        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;

        #endregion

        #region ctor

        public CategoryController(ICategoryService categoryService, IProductService productService)
        {
            _categoryService = categoryService;
            _productService = productService;
        }

        #endregion

        #region List

        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public ActionResult List()
        {
            return View(new CategoryListModel());
        }

        [HttpPost]
        public ActionResult List(DataRequest request, CategoryListModel search)
        {
            var categories = _categoryService.GetAllCategories(search.SearchCategoryName,
                request.Page - 1, request.PageSize);
            var model = new DataResult
            {
                Data = categories.Select(x =>
                {
                    var categoryModel = x.ToCategoryModel();
                    return categoryModel;
                }),
                Total = categories.TotalCount
            };
            return Json(model);
        }
       
        #endregion

        #region CRUD

        // GET: Admin/Category/Create
        public ActionResult Create()
        {
            CategoryModel model = new CategoryModel();
            return View(model);
        }

        // POST: Admin/Category/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Description,PictureId,Deleted,ShowOnHomePage,CreatedOnUtc,UpdatedOnUtc,ParentCategoryId")] Category category)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            return View();
        }

        // GET: Admin/Category/Edit/5 
        public ActionResult Edit(int id)
        {
            var category = _categoryService.GetCategoryById(id);
            if (category == null || category.Deleted)
                return RedirectToAction("List");

            var model = category.ToCategoryModel();

            return View(model);
        }

        // POST: Admin/Category/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,PictureId,Deleted,ShowOnHomePage,CreatedOnUtc,UpdatedOnUtc,ParentCategoryId")] Category category)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        // GET: Admin/Category/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View();
        }

        // POST: Admin/Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            return RedirectToAction("Index");
        }

        #endregion

        #region Pruduct



        #endregion
    }
}
