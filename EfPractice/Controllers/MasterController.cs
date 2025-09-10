using EfPractice.Models;
using EfPractice.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EfPractice.Controllers
{
    public class MasterController : Controller
    {
        private readonly IMaster _master;
        public MasterController(IMaster Master)
        {
            _master = Master;


        }
        #region Inventory Head
        [HttpGet]
        public async Task<IActionResult> InventoryHead()
        {
            var abc = await _master.GetItemHeaderAsync();
            return View(abc);
        }

        [HttpGet]
        public async Task<IActionResult> GetInventoryHeadList()
        {
            var items = await _master.GetItemHeaderAsync(); // Fetch your items from the database or service
            return Json(items); // Return items as JSON
        }
        [HttpPost]
        public async Task<IActionResult> UpdateInventoryHead([FromBody] Imh model)
        {
            var Result = await _master.UpdateItemHeaderAsync(model);

            if (Result)
            {

                return Ok(new
                {
                    success = true,
                    message = "Item updated successfully.",

                });


            }
            else
            {
                return Ok(new
                {
                    success = false,
                    message = "Item  not updated successfully.",

                });
            }

        }

        [HttpPost]
        public async Task<IActionResult> AddInventoryHead([FromBody] Imh model)
        {
            var Result = await _master.AddItemHeaderAsync(model);

            if (Result)
            {

                return Ok(new
                {
                    success = true,
                    message = "Item Head Added successfully.",

                });


            }
            else
            {
                return Ok(new
                {
                    success = false,
                    message = "Item Head not Added successfully.",

                });
            }
        }

        #endregion


        #region ItemCatergory

        [HttpGet]
        public async Task<IActionResult> ItemCatergoryRegistrarion()
        {
            var Result = await _master.GetHeaderListAsync();


            return View(Result);
        }


        [HttpPost]
        public async Task<IActionResult> ADDCatergoryRegistrarion([FromBody] Cate model)
        {
             var Result = await _master.ADDItemCatergoryRegistrarionAsync(model);

            if (Result)
            {

                return Ok(new
                {
                    success = true,
                    message = "Item Catergory Added successfully.",

                });


            }
            else
            {
                return Ok(new
                {
                    success = false,
                    message = "Item Catergory not Added successfully.",

                });

            }
        }

        [HttpPost]
        public async Task<IActionResult> UPDATECatergoryRegistrarion([FromBody] Cate model)
        {
            var Result = await _master.UPDATEItemCatergoryRegistrarionAsync(model);

            if (Result)
            {

                return Ok(new
                {
                    success = true,
                    message = "Item Catergory Update successfully.",

                });


            }
            else
            {
                return Ok(new
                {
                    success = false,
                    message = "Item Catergory not Update successfully.",

                });

            }
        }


        [HttpGet]
        public async Task<IActionResult> GetCatergoryRegistrarion()
        {
            var items = await _master.GETItemCatergoryRegistrarionAsync(); // Fetch your items from the database or service
            return Json(items); // Return items as JSON
        }

        #endregion


        #region Item Registration

        [HttpGet]
        public async Task<IActionResult> ItemRegistration()
        {
            var Result = await _master.GetCategoryListAsync();


            return View(Result);
        }

        [HttpPost]
        public async Task<IActionResult> AddItemRegistration([FromBody] Item model)
        {
            var Result = await _master.AddItemRegistration(model);

            if (Result)
            {

                return Ok(new
                {
                    success = true,
                    message = "Item Added successfully.",

                });


            }
            else
            {
                return Ok(new
                {
                    success = false,
                    message = "Item not Added successfully.",

                });

            }
        }



        [HttpPost]
        public async Task<IActionResult> UpdateItemRegistration([FromBody] Item model)
        {
            var Result = await _master.UpdateItemRegistration(model);

            if (Result)
            {

                return Ok(new
                {
                    success = true,
                    message = "Item Update successfully.",

                });


            }
            else
            {
                return Ok(new
                {
                    success = false,
                    message = "Item not Update successfully.",

                });

            }
        }




        [HttpGet]
        public async Task<IActionResult> GetItemRegistration()
        {
            var items = await _master.GetItemRegistration(); // Fetch your items from the database or service
            return Json(items); // Return items as JSON
        }

        #endregion


        #region Supplier


        [HttpGet]
        public async Task<IActionResult> Supplier()
        {
            var abc = await _master.GetSupplierAsync();
            return View(abc);
        }

        [HttpGet]
        public async Task<IActionResult> GetSupplierList()
        {
            var items = await _master.GetSupplierAsync(); // Fetch your items from the database or service
            return Json(items); // Return items as JSON
        }
        [HttpPost]
        public async Task<IActionResult> UpdateSupplier([FromBody] Supplier model)
        {
            var Result = await _master.UpdateSupplierAsync(model);

            if (Result)
            {

                return Ok(new
                {
                    success = true,
                    message = "Supplier updated successfully.",

                });


            }
            else
            {
                return Ok(new
                {
                    success = false,
                    message = "Supplier  not updated successfully.",

                });
            }

        }

        [HttpPost]
        public async Task<IActionResult> AddSupplier([FromBody] Supplier model)
        {
            var Result = await _master.AddSupplierAsync(model);

            if (Result)
            {

                return Ok(new
                {
                    success = true,
                    message = "Supplier Added successfully.",

                });


            }
            else
            {
                return Ok(new
                {
                    success = false,
                    message = "Supplier not Added successfully.",

                });
            }
        }






        #endregion


        #region Party

        [HttpGet]
        public async Task<IActionResult> Party()
        {
            var abc = await _master.GetPartyAsync();
            return View(abc);
        }

        [HttpGet]
        public async Task<IActionResult> GetPartyList()
        {
            var items = await _master.GetPartyAsync(); // Fetch your items from the database or service
            return Json(items); // Return items as JSON
        }
        [HttpPost]
        public async Task<IActionResult> UpdateParty([FromBody] Party model)
        {
            var Result = await _master.UpdatePartyAsync(model);

            if (Result)
            {

                return Ok(new
                {
                    success = true,
                    message = "Party updated successfully.",

                });


            }
            else
            {
                return Ok(new
                {
                    success = false,
                    message = "Party  not updated successfully.",

                });
            }

        }

        [HttpPost]
        public async Task<IActionResult> AddParty([FromBody] Party model)
        {
            var Result = await _master.AddPartyAsync(model);

            if (Result)
            {

                return Ok(new
                {
                    success = true,
                    message = "Party Added successfully.",

                });


            }
            else
            {
                return Ok(new
                {
                    success = false,
                    message = "Party not Added successfully.",

                });
            }
        }

        #endregion


        #region Head
        public async Task<IActionResult> UpdateHead([FromBody] Head model)
        {
            var Result = await _master.UpdateHeadAsync(model);

            if (Result)
            {

                return Ok(new
                {
                    success = true,
                    message = "Item updated successfully.",

                });


            }
            else
            {
                return Ok(new
                {
                    success = false,
                    message = "Item  not updated successfully.",

                });
            }

        }

        [HttpPost]
        public async Task<IActionResult> AddHead([FromBody] Head model)
        {
            var Result = await _master.AddHeadAsync(model);

            if (Result)
            {

                return Ok(new
                {
                    success = true,
                    message = " Head Added successfully.",

                });


            }
            else
            {
                return Ok(new
                {
                    success = false,
                    message = "Head not Added successfully.",

                });
            }
        }



        [HttpGet]
        public async Task<IActionResult> GetHeadList()
        {
            var items = await _master.GetHeadListAsync(); // Fetch your items from the database or service
            return Json(items); // Return items as JSON
        }

        #endregion



    }
}
