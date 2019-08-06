﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FamilyNet.Models;
using FamilyNet.Models.Interfaces;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using FamilyNet.Models.ViewModels;
using FamilyNet.Infrastructure;

namespace FamilyNet.Controllers
{
    [Authorize]
    public class RepresentativesController : BaseController
    {
        #region Private fields

        private PersonSearchModel _searchModel;
        //private PersonFilter _personFilter;

        #endregion

        #region Ctor

        public RepresentativesController(IUnitOfWorkAsync unitOfWork)
            : base(unitOfWork)
        {
            //_personFilter = new PersonFilter();
        }
        #endregion

        #region Methods

        // GET: Representatives
        [AllowAnonymous]
        public async Task<IActionResult> Index(int id, 
            /*FilterUtility.FilterParams*/PersonSearchModel searchModel)//TODO: Representative search model
        {
            IQueryable<Representative> representatives = _unitOfWorkAsync.Representatives.GetAll();
            //sort -> IEnumerable

            #region Manual creating filterParams
            FilterUtility.FilterParams searchParamFullName = new FilterUtility.FilterParams();
            searchParamFullName.ColumnName = "FullNameString";//TODO: AlPa -> ?? Check working
            searchParamFullName.FilterValue = searchModel.FullNameString;
            searchParamFullName.FilterOptions = FilterUtility.FilterOptions.Contains;

            FilterUtility.FilterParams searchParamRate = new FilterUtility.FilterParams();
            searchParamRate.ColumnName = "Rating";//TODO: AlPa -> ?? Check working
            searchParamRate.FilterValue = searchModel.RatingNumber;
            searchParamRate.FilterOptions = FilterUtility.FilterOptions.IsGreaterThanOrEqualTo;
            IEnumerable<FilterUtility.FilterParams> enuParams =
                new List<FilterUtility.FilterParams>() { searchParamFullName, searchParamRate };
            #endregion

            PaginatedInputModel inm = new PaginatedInputModel();
            //inm.FilterParam = enuParams;
            //representatives = GetFilteredSortedPaginatedList(representatives, enuParams);

            //TODO: AlPa ->  CAST:
            //representatives = (IQueryable<Representative>)representatives.GetFiltered(searchModel); //фильтрация
            //representatives = (IQueryable<Representative>)_personFilter.GetFiltered(representatives, searchModel);
            //representatives = GetFiltered(searchModel, representatives);

            if (id == 0)
                return View(await representatives.ToListAsync());

            if (id > 0)
                representatives = representatives.Where(x => x.Orphanage.ID.Equals(id));

            return View(await representatives.ToListAsync());
        }

        //private IQueryable<Representative> GetFiltered(PersonSearchModel searchModel,
        //    IQueryable<Representative> orphans)
        //{
        //    if (searchModel != null)
        //    {
        //        _searchModel = searchModel;

        //        if (!string.IsNullOrEmpty(searchModel.FullNameString))
        //            orphans = orphans.Where(x => Contains(x.FullName));

        //        if (searchModel.RatingNumber > 0)
        //            orphans = orphans.Where(x => x.Rating == searchModel.RatingNumber);
        //    }

        //    return orphans;
        //}

        //private bool Contains(FullName fullname)
        //{
        //    foreach (var word in _searchModel.FullNameString.Split())
        //    {
        //        string wordUpper = word.ToUpper();

        //        if (fullname.Name.ToUpper().Contains(wordUpper)
        //                || fullname.Surname.ToUpper().Contains(wordUpper)
        //                || fullname.Patronymic.ToUpper().Contains(wordUpper))
        //            return true;
        //    }

        //    return false;
        //}

        // GET: Representatives/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var representative = await _unitOfWorkAsync.Representatives.GetById((int)id);

            if (representative == null)
                return NotFound();

            return View(representative);
        }
        [Authorize(Roles = "Admin")]
        // GET: Representatives/Create
        public async Task<IActionResult> Create()
        {
            List<Orphanage> orphanages = await _unitOfWorkAsync.Orphanages.GetAll()
                .OrderBy(o => o.Name).ToListAsync();
            ViewBag.Orphanages = new SelectList(orphanages, "ID", "Name");

            return View();
        }

        // POST: Representatives/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(
            [Bind("FullName,Birthday,Rating,Avatar,Orphanage")] Representative representative,
            int id, IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var fileName = Path.GetRandomFileName();
                fileName = Path.ChangeExtension(fileName, ".jpg");
                var filePath = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot\\representatives", fileName);
                using (var fileSteam = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileSteam);
                }
                representative.Avatar = fileName;
            }

            if (ModelState.IsValid)
            {
                var orphanage = await _unitOfWorkAsync.Orphanages.GetById(id);
                representative.Orphanage = orphanage;

                await _unitOfWorkAsync.Representatives.Create(representative);
                await _unitOfWorkAsync.Representatives.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(representative);
        }

        // GET: Representatives/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            List<Orphanage> orphanages = _unitOfWorkAsync.Orphanages.GetAll()
                .OrderBy(o => o.Name).ToList();
            ViewBag.Orphanages = new SelectList(orphanages, "ID", "Name");

            if (id == null)
                return NotFound();

            var representative = await _unitOfWorkAsync.Representatives.GetById((int)id);

            if (representative == null)
                return NotFound();

            return View(representative);
        }

        // POST: Representatives/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id,
            [Bind("ID,FullName,Birthday,Rating,Avatar,Orphanage")] Representative representative,
            int orphanageId, IFormFile file)
        {
            if (id != representative.ID)
                return NotFound();

            if (file != null && file.Length > 0)
            {
                var fileName = Path.GetRandomFileName();
                fileName = Path.ChangeExtension(fileName, ".jpg");
                var filePath = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot\\representatives", fileName);
                using (var fileSteam = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileSteam);
                }
                representative.Avatar = fileName;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var orphanage = await _unitOfWorkAsync.Orphanages.GetById(orphanageId);
                    representative.Orphanage = orphanage;

                    var representativeToEdit = await _unitOfWorkAsync.Representatives.GetById(representative.ID);
                    representativeToEdit.CopyState(representative);
                    _unitOfWorkAsync.Representatives.Update(representativeToEdit);
                    _unitOfWorkAsync.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RepresentativeExists(representative.ID))
                        return NotFound();
                    else
                        throw; //TODO: AlPa -> Loging
                }

                return RedirectToAction(nameof(Index));
            }

            return View(representative);
        }

        // GET: Representatives/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var representative = await _unitOfWorkAsync.Representatives.GetById((int)id);
            if (representative == null)
                return NotFound();

            return View(representative);
        }

        // POST: Representatives/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var representative = await _unitOfWorkAsync.Representatives.GetById((int)id);
            await _unitOfWorkAsync.Representatives.Delete(representative.ID);
            _unitOfWorkAsync.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool RepresentativeExists(int id)
        {
            return _unitOfWorkAsync.Representatives.GetById(id) != null;
        }

        public async Task<PaginatedList<PersonSearchModel>> GetFilteredSortedPaginatedList(List<PersonSearchModel> persons, PaginatedInputModel pagingParams)
        {
            List<PersonSearchModel> sampleList = persons;


            #region [Filter]  
            if (pagingParams != null && pagingParams.FilterParam.Any())
            {
                sampleList = FilterUtility.Filter<PersonSearchModel>
                    .GetFilteredData(pagingParams.FilterParam, sampleList).ToList() ?? sampleList;
            }
            #endregion

            #region [Sorting]  
            if (pagingParams != null && pagingParams.SortingParams.Count() > 0
                    && Enum.IsDefined(typeof(SortingUtility.SortOrders),
                    pagingParams.SortingParams.Select(x => x.SortOrder)))
            {
                sampleList = SortingUtility.Sorting<PersonSearchModel>
                    .SortData(sampleList, pagingParams.SortingParams).ToList();
            }
            #endregion

            #region [Grouping]  
            if (pagingParams != null && pagingParams.GroupingColumns != null
                    && pagingParams.GroupingColumns.Count() > 0)
            {
                sampleList = SortingUtility.Sorting<PersonSearchModel>
                    .GroupingData(sampleList, pagingParams.GroupingColumns).ToList() ?? sampleList;
            }
            #endregion

            #region [Paging]  
            return await PaginatedList<PersonSearchModel>
                .CreateAsync(sampleList, pagingParams.PageNumber, pagingParams.PageSize);
            #endregion
        }

        #endregion
    }
}
