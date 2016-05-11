using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

using Bong.Core.Helper;
using Bong.Core.Infrastructure;
using Bong.Core.Configuration;
using Bong.Core.Data;
using Bong.Core.Domain.Customers;
using Bong.Core.Domain.Goods;
using Bong.Core.Domain.Media;
using Bong.Core.Domain.Orders;
using Bong.Data;
using Bong.Services.Media;

namespace Bong.Services.Install
{
    public class InstallService : IInstallService
    {
        #region Fields

        private readonly IWebHelper _webHelper;
        private readonly IDbContext _dbContext;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<Picture> _pictureRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<Product> _productRepository;
        
        #endregion

        #region ctor

        public InstallService(IDbContext dbContext, IRepository<Customer> customerRepository, 
            IRepository<Picture> pictureRepository, IRepository<Category> categoryRepository, 
            IRepository<Product> productRepository, IWebHelper webHelper)
        {
            _dbContext = dbContext;
            _webHelper = webHelper;
            _customerRepository = customerRepository;
            _categoryRepository = categoryRepository;
            _pictureRepository = pictureRepository;
            _productRepository = productRepository;
        }

        #endregion

        #region Implementation of IIstallService

        public virtual void InstallData(string defaultUserEmail="admin@bong.com", 
            string defaultUserPassword="admin", bool installSampleData = true)
        {
            bool emptyDatabase = IsEmptyDatabase();

            if (installSampleData && emptyDatabase)
            {
                InstallCustomers(defaultUserEmail, defaultUserPassword);

                InstallCategories();

                InstallProducts(defaultUserEmail);
            } 
        }

        #endregion

        #region Utility

        protected virtual bool IsEmptyDatabase()
        {
            IList<Customer> customers = _customerRepository.Table.ToList();

            return customers.Count() == 0;
        }

        protected virtual void InstallCustomers(string adminEmail, string adminPassword)
        {
            var adminUser = new Customer()
            {
                CustomerGuid = Guid.NewGuid(),
                Email = adminEmail,
                Username = adminEmail,
                PasswordHashCode = adminPassword.GetHashCode(),
                Active = true,
                UserType = UserType.Admin,
                CreatedOnUtc = DateTime.UtcNow,
            };
            _customerRepository.Insert(adminUser);
        }

        protected virtual void InstallCategories()
        {
            // pictures path
            var pictureService = TheSystem.Current.Resolve<IPictureService>();
            var sampleImagesPath = _webHelper.MapPath("~/content/images/sample_pics/");

            //categories
            var categoryBooks = new Category
            {
                Name = "Books",
                PictureId = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "category_book.jpg"), "image/pjpeg", true).Id,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow
            };
            _categoryRepository.Insert(categoryBooks);

            var categoryComputers = new Category
            {
                Name = "Computers",
                PictureId = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "category_computers.jpg"), "image/pjpeg", true).Id,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow
            };
            _categoryRepository.Insert(categoryComputers);


            var categoryDesktops = new Category
            {
                Name = "Desktops",
                ParentCategoryId = categoryComputers.Id,
                PictureId = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "category_desktops.jpg"), "image/pjpeg", true).Id,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow
            };
            _categoryRepository.Insert(categoryDesktops);


            var categoryNotebooks = new Category
            {
                Name = "Notebooks",
                ParentCategoryId = categoryComputers.Id,
                PictureId = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "category_notebooks.jpg"), "image/pjpeg", true).Id,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow
            };
            _categoryRepository.Insert(categoryNotebooks);


            var categoryAccessories = new Category
            {
                Name = "Accessories",
                ParentCategoryId = categoryComputers.Id,
                PictureId = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "category_accessories.jpg"), "image/pjpeg", true).Id,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow
            };
            _categoryRepository.Insert(categoryAccessories);


            var categorySoftware = new Category
            {
                Name = "Software & Games",
                ParentCategoryId = categoryComputers.Id,
                PictureId = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "category_software.jpg"), "image/pjpeg", true).Id,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow
            };
            _categoryRepository.Insert(categorySoftware);

            var categoryElectronics = new Category
            {
                Name = "Electronics",
                PictureId = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "category_electronics.jpg"), "image/pjpeg", true).Id,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow
            };
            _categoryRepository.Insert(categoryElectronics);


            var categoryCameraPhoto = new Category
            {
                Name = "Camera, photo",
                ParentCategoryId = categoryElectronics.Id,
                PictureId = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "category_camera_photo.jpg"), "image/pjpeg", true).Id,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow
            };
            _categoryRepository.Insert(categoryCameraPhoto);


            var categoryCellPhones = new Category
            {
                Name = "Cell phones",
                ParentCategoryId = categoryElectronics.Id,
                PictureId = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "category_cell_phones.jpg"), "image/pjpeg", true).Id,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow
            };
            _categoryRepository.Insert(categoryCellPhones);

            var categoryApparelShoes = new Category
            {
                Name = "Apparel & Shoes",
                PictureId = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "category_apparel_shoes.jpg"), "image/pjpeg", true).Id,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow
            };
            _categoryRepository.Insert(categoryApparelShoes);

            var categoryJewelry = new Category
            {
                Name = "Jewelry",
                PictureId = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "category_jewelry.jpg"), "image/pjpeg", true).Id,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow
            };
            _categoryRepository.Insert(categoryJewelry);
        }

        protected virtual void InstallProducts(string userName)
        {
            //default customer/user
            var defaultCustomer = _customerRepository.Table.FirstOrDefault(x => x.Email == userName);
            if (defaultCustomer == null)
                throw new Exception("Cannot load default customer");

            //pictures
            var pictureService = TheSystem.Current.Resolve<IPictureService>();
            var sampleImagesPath = _webHelper.MapPath("~/content/images/sample_pics/");

            //products
            var allProducts = new List<Product>();
            var productAcerAspireOne = new Product()
            {
                Name = "Acer Aspire One 8.9\" Mini-Notebook Case - (Black)",
                ShortDescription = "Acer Aspire One 8.9\" Mini-Notebook and 6 Cell Battery model (AOA150-1447)",
                Description = "<p>Acer Aspire One 8.9&quot; Memory Foam Pouch is the perfect fit for Acer Aspire One 8.9&quot;. This pouch is made out of premium quality shock absorbing memory form and it provides extra protection even though case is very light and slim. This pouch is water resistant and has internal supporting bands for Acer Aspire One 8.9&quot;. Made In Korea.</p>",
                Price = 21.6M,
                Weight = 2,
                Length = 2,
                Width = 2,
                Height = 3,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow
            };
            allProducts.Add(productAcerAspireOne);
            productAcerAspireOne.ProductCategories.Add(new ProductCategory()
            {
                Category = _categoryRepository.Table.Single(c => c.Name == "Accessories"),
                ShowOrder = 1,
            });
            productAcerAspireOne.ProductPictures.Add(new ProductPicture()
            {
                Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_AcerAspireOne_1.jpeg"), "image/jpeg", true),
                ShowOrder = 1,
            });
            productAcerAspireOne.ProductPictures.Add(new ProductPicture()
            {
                Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_AcerAspireOne_2.jpeg"), "image/jpeg", true),
                ShowOrder = 2,
            });
            _productRepository.Insert(productAcerAspireOne);

            var productAdidasShoe = new Product()
            {
                Name = "Adidas Women's Cicinnurus Running Shoes (BNWB, US-6)",
                ShortDescription = "Now there are even more reasons to love this training favorite. An improved last, new step-in sockliner and the smooth control of 3-D ForMotion™ deliver a natural, balanced touchdown that feels better than ever.",
                Description = "<p>Slip on shoes open mid foot in bright coral and white tone.Lightweight, stretchable upper for flexibility, with focus on dance specific movements. Comfortable textile lining and flexible outsole with rubber inserts for grip.</p>",
                Price = 40M,
                Weight = 2,
                Length = 2,
                Width = 2,
                Height = 2,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow
            };
            allProducts.Add(productAdidasShoe);
            productAdidasShoe.ProductCategories.Add(new ProductCategory()
            {
                Category = _categoryRepository.Table.Single(c => c.Name == "Apparel & Shoes"),
                ShowOrder = 1,
            });
            productAdidasShoe.ProductPictures.Add(new ProductPicture()
            {
                Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_AdidasShoe_1.jpg"), "image/pjpeg", true),
                ShowOrder = 1,
            });
            productAdidasShoe.ProductPictures.Add(new ProductPicture()
            {
                Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_AdidasShoe_2.jpg"), "image/pjpeg", true),
                ShowOrder = 2,
            });
            _productRepository.Insert(productAdidasShoe);

            var productAdobePhotoshop = new Product()
            {
                Name = "Office 365 Home (PC/Mac) - English",
                ShortDescription = "Easily view and edit your document.",
                Description = "<p>Office 365 Home is more personalized and connected way to access your must-have Microsoft Office applications and documents. Compatible with Windows and Mac, this 1-year subscription software keeps you connected at home and on the go to Microsoft Office applications on up to 5 compatible devices. </p>",
                Price = 75M,
                Weight = 2,
                Length = 2,
                Width = 2,
                Height = 3,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow
            };
            allProducts.Add(productAdobePhotoshop);
            productAdobePhotoshop.ProductCategories.Add(new ProductCategory()
            {
                Category = _categoryRepository.Table.Single(c => c.Name == "Software & Games"),
                ShowOrder = 1,
            });
            productAdobePhotoshop.ProductPictures.Add(new ProductPicture()
            {
                Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_Office365.jpg"), "image/pjpeg", true),
                ShowOrder = 1,
            });
            _productRepository.Insert(productAdobePhotoshop);

            var productApcUps = new Product()
            {
                Name = "APC Back-UPS RS 800VA - UPS - 800 VA - UPS battery - lead acid ( BR800BLK )",
                ShortDescription = "APC Back-UPS RS, 800VA/540W, Input 120V/Output 120V, Interface Port USB. ",
                Description = "<p>The Back-UPS RS offers high performance protection for your business and office computer systems. It provides abundant battery backup power, allowing you to work through medium and extended length power outages. It also safeguards your equipment from damaging surges and spikes that travel along utility, phone and network lines. A distinguishing feature of the Back-UPS RS is automatic voltage regulation (AVR). AVR instantly adjusts both low and high voltages to safe levels, so you can work indefinitely during brownouts and overvoltage situations, saving the battery for power outages when you need it most. Award-winning shutdown software automatically powers down your computer system in the event of an extended power outage.</p>",
                Price = 75M,
                Weight = 2,
                Length = 2,
                Width = 2,
                Height = 3,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow
            };
            allProducts.Add(productApcUps);
            productApcUps.ProductCategories.Add(new ProductCategory()
            {
                Category = _categoryRepository.Table.Single(c => c.Name == "Accessories"),
                ShowOrder = 1,
            });
            productApcUps.ProductPictures.Add(new ProductPicture()
            {
                Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_ApcUps.jpeg"), "image/jpeg", true),
                ShowOrder = 1,
            });
            _productRepository.Insert(productApcUps);

            var productArrow = new Product()
            {
                Name = "Arrow Men's Wrinkle Free Pinpoint Solid Long Sleeve",
                ShortDescription = "",
                Description = "<p>This Wrinkle Free Pinpoint Long Sleeve Dress Shirt needs minimum ironing. It is a great product at a great value!</p>",
                Price = 24M,
                Weight = 4,
                Length = 3,
                Width = 3,
                Height = 3,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow
            };
            allProducts.Add(productArrow);
            productArrow.ProductCategories.Add(new ProductCategory()
            {
                Category = _categoryRepository.Table.Single(c => c.Name == "Apparel & Shoes"),
                ShowOrder = 1,
            });
            productArrow.ProductPictures.Add(new ProductPicture()
            {
                Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_arrow.jpg"), "image/pjpeg", true),
                ShowOrder = 1,
            });
            _productRepository.Insert(productArrow);

            var productAsusPc1000 = new Product()
            {
                Name = "ASUS X551CA 15.6 Laptop - Black",
                ShortDescription = "Super Hybrid Engine offers a choice of performance and power consumption modes for easy adjustments according to various needs",
                Description = "<p>Style and high performance collide in the ASUS X Series 15.6 laptop. Equipped with an Intel Pentium processor, 6GB RAM and a 750GB hard drive, this laptop is designed for entertainment and everyday multitasking. ASUS SonicMaster Lite technology makes your music, games and movies sound richly detailed, while IceCool technology keeps the palm rests cool for long typing sessions.</p>",
                Price = 1480M,
                Weight = 3,
                Length = 3,
                Width = 2,
                Height = 2,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow
            };
            allProducts.Add(productAsusPc1000);
            productAsusPc1000.ProductCategories.Add(new ProductCategory()
            {
                Category = _categoryRepository.Table.Single(c => c.Name == "Notebooks"),
                ShowOrder = 1,
            });
            productAsusPc1000.ProductPictures.Add(new ProductPicture()
            {
                Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_asuspc001.jpg"), "image/pjpeg", true),
                ShowOrder = 1,
            });
            _productRepository.Insert(productAsusPc1000);

            var productAsusPc900 = new Product()
            {
                Name = "ASUS Eee PC 900HA 8.9-Inch Netbook Black",
                ShortDescription = "High Speed Connectivity Anywhere with Wi-Fi 802.11b/g.",
                Description = "<p>Much more compact than a standard-sized notebook and weighing just 2.5 pounds, the Eee PC 900HA is perfect for students toting to school or road warriors packing away to Wi-Fi hotspots. In addition to the 160 GB hard disk drive (HDD), the Eee PC 900HA also features 1 GB of RAM, VGA-resolution webcam integrated into the bezel above the LCD, 54g Wi-Fi networking (802.11b/g), multiple USB ports, SD memory card slot, a VGA output for connecting to a monitor, and up to 10 GB of online storage (complimentary for 18 months).</p><p>It comes preinstalled with the Microsoft Windows XP Home operating system, which offers more experienced users an enhanced and innovative experience that incorporates Windows Live features like Windows Live Messenger for instant messaging and Windows Live Mail for consolidated email accounts on your desktop. Complementing this is Microsoft Works, which equips the user with numerous office applications to work efficiently.</p>",
                Price = 1500M,
                Weight = 7,
                Length = 7,
                Width = 7,
                Height = 7,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow
            };
            allProducts.Add(productAsusPc900);
            productAsusPc900.ProductCategories.Add(new ProductCategory()
            {
                Category = _categoryRepository.Table.Single(c => c.Name == "Notebooks"),
                ShowOrder = 1,
            });
            productAsusPc900.ProductPictures.Add(new ProductPicture()
            {
                Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_asuspc900.jpeg"), "image/jpeg", true),
                ShowOrder = 1,
            });
            _productRepository.Insert(productAsusPc900);

            var productBestGrillingRecipes = new Product()
            {
                Name = "Best Grilling Recipes",
                ShortDescription = "More Than 100 Regional Favorites Tested and Perfected for the Outdoor Cook (Hardcover)",
                Description = "<p>Take a winding cross-country trip and you'll discover barbecue shacks with offerings like tender-smoky Baltimore pit beef and saucy St. Louis pork steaks. To bring you the best of these hidden gems, along with all the classics, the editors of Cook's Country magazine scoured the country, then tested and perfected their favorites. HEre traditions large and small are brought into the backyard, from Hawaii's rotisserie favorite, the golden-hued Huli Huli Chicken, to fall-off-the-bone Chicago Barbecued Ribs. In Kansas City, they're all about the sauce, and for our saucy Kansas City Sticky Ribs, we found a surprise ingredient-root beer. We also tackle all the best sides. <br /><br />Not sure where or how to start? This cookbook kicks off with an easy-to-follow primer that will get newcomers all fired up. Whether you want to entertain a crowd or just want to learn to make perfect burgers, Best Grilling Recipes shows you the way.</p>",
                Price = 27M,
                OldPrice = 30M,
                Weight = 2,
                Length = 2,
                Width = 2,
                Height = 2,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow
            };
            allProducts.Add(productBestGrillingRecipes);
            productBestGrillingRecipes.ProductCategories.Add(new ProductCategory()
            {
                Category = _categoryRepository.Table.Single(c => c.Name == "Books"),
                ShowOrder = 1,
            });
            productBestGrillingRecipes.ProductPictures.Add(new ProductPicture()
            {
                Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_BestGrillingRecipes.jpeg"), "image/jpeg", true),
                ShowOrder = 1,
            });
            _productRepository.Insert(productBestGrillingRecipes);

            var productDiamondHeart = new Product()
            {
                Name = "Black & White Diamond Heart",
                ShortDescription = "Heart Pendant 1/4 Carat (ctw) in Sterling Silver",
                Description = "<p>Bold black diamonds alternate with sparkling white diamonds along a crisp sterling silver heart to create a look that is simple and beautiful. This sleek and stunning 1/4 carat (ctw) diamond heart pendant which includes an 18 inch silver chain, and a free box of godiva chocolates makes the perfect Valentine's Day gift.</p>",
                Price = 130M,
                Weight = 2,
                Length = 2,
                Width = 2,
                Height = 2,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow
            };
            allProducts.Add(productDiamondHeart);
            productDiamondHeart.ProductCategories.Add(new ProductCategory()
            {
                Category = _categoryRepository.Table.Single(c => c.Name == "Jewelry"),
                ShowOrder = 1,
            });
            productDiamondHeart.ProductPictures.Add(new ProductPicture()
            {
                Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_DiamondHeart.jpg"), "image/pjpeg", true),
                ShowOrder = 1,
            });
            _productRepository.Insert(productDiamondHeart);

            var productBlackBerry = new Product()
            {
                Name = "BlackBerry Bold 9000 Phone, Black (AT&T)",
                ShortDescription = "Global Blackberry messaging smartphone with quad-band GSM",
                Description = "<p>Keep yourself on track for your next meeting with turn-by-turn directions via the AT&amp;T Navigator service, which is powered by TeleNav and provides spoken or text-based turn-by-turn directions with automatic missed turn rerouting and a local business finder service in 20 countries. It also supports AT&amp;T mobile music services and access to thousands of video clips via Cellular Video. Other features include a 2-megapixel camera/camcorder, Bluetooth for handsfree communication, 1 GB of internal memory with MicroSD expansion (up to 32 GB), multi-format audio/video playback, and up to 4.5 hours of talk time.</p><p>The Blackberry Bold also comes with free access to AT&amp;T Wi-Fi Hotspots, available at more than 17,000 locations nationwide including Starbucks. The best part is that you do'nt need to sign up for anything new to use this service--Wi-Fi access for is included in all Blackberry Personal and Enterprise Rate Plans. (You must subscribe to a Blackberry Data Rate Plan to access AT&amp;T Wi-Fi Hotspots.) Additionally, the Blackberry Bold is the first RIM device that supports AT&amp;T Cellular Video (CV).</p>",
                Price = 245M,
                Weight = 2,
                Length = 2,
                Width = 2,
                Height = 2,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow
            };
            allProducts.Add(productBlackBerry);
            productBlackBerry.ProductCategories.Add(new ProductCategory()
            {
                Category = _categoryRepository.Table.Single(c => c.Name == "Cell phones"),
                ShowOrder = 1,
            });
            productBlackBerry.ProductPictures.Add(new ProductPicture()
            {
                Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_BlackBerry.jpeg"), "image/jpeg", true),
                ShowOrder = 1,
            });
            _productRepository.Insert(productBlackBerry);

            var productBuildComputer = new Product()
            {
                Name = "Build your own computer",
                ShortDescription = "Build it",
                Description = "<p>Fight back against cluttered workspaces with the stylish Sony VAIO JS All-in-One desktop PC, featuring powerful computing resources and a stunning 20.1-inch widescreen display with stunning XBRITE-HiColor LCD technology. The silver Sony VAIO VGC-JS110J/S has a built-in microphone and MOTION EYE camera with face-tracking technology that allows for easy communication with friends and family. And it has a built-in DVD burner and Sony's Movie Store software so you can create a digital entertainment library for personal viewing at your convenience. Easy to setup and even easier to use, this JS-series All-in-One includes an elegantly designed keyboard and a USB mouse.</p>",
                Price = 1200M,
                Weight = 2,
                Length = 2,
                Width = 2,
                Height = 2,
                ShowOnHomePage = true,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow
            };
            allProducts.Add(productBuildComputer);
            productBuildComputer.ProductCategories.Add(new ProductCategory()
            {
                Category = _categoryRepository.Table.Single(c => c.Name == "Desktops"),
                ShowOrder = 1,
            });
            productBuildComputer.ProductPictures.Add(new ProductPicture()
            {
                Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_Desktops_1.jpeg"), "image/jpeg", true),
                ShowOrder = 1,
            });
            productBuildComputer.ProductPictures.Add(new ProductPicture()
            {
                Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_Desktops_2.jpeg"), "image/jpeg", true),
                ShowOrder = 2,
            });
            productBuildComputer.ProductPictures.Add(new ProductPicture()
            {
                Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_Desktops_3.jpeg"), "image/jpeg", true),
                ShowOrder = 3,
            });
            _productRepository.Insert(productBuildComputer);

            var productCanonCamera = new Product()
            {
                Name = "Canon Digital SLR Camera",
                ShortDescription = "12.2-megapixel CMOS sensor captures enough detail for poster-size, photo-quality prints",
                Description = "<p>For stunning photography with point and shoot ease, look no further than Canon&rsquo;s EOS Rebel XSi. The EOS Rebel XSi brings staggering technological innovation to the masses. It features Canon&rsquo;s EOS Integrated Cleaning System, Live View Function, a powerful DIGIC III Image Processor, plus a new 12.2-megapixel CMOS sensor and is available in a kit with the new EF-S 18-55mm f/3.5-5.6 IS lens with Optical Image Stabilizer. The EOS Rebel XSi&rsquo;s refined, ergonomic design includes a new 3.0-inch LCD monitor, compatibility with SD and SDHC memory cards and new accessories that enhance every aspect of the photographic experience.</p>",
                Price = 670M,
                Weight = 2,
                Length = 2,
                Width = 2,
                Height = 2,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow
            };
            allProducts.Add(productCanonCamera);
            productCanonCamera.ProductCategories.Add(new ProductCategory()
            {
                Category = _categoryRepository.Table.Single(c => c.Name == "Camera, photo"),
                ShowOrder = 1,
            });
            productCanonCamera.ProductPictures.Add(new ProductPicture()
            {
                Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_CanonCamera_1.jpeg"), "image/jpeg", true),
                ShowOrder = 1,
            });
            productCanonCamera.ProductPictures.Add(new ProductPicture()
            {
                Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_CanonCamera_2.jpeg"), "image/jpeg", true),
                ShowOrder = 2,
            });
            _productRepository.Insert(productCanonCamera);
            
            var productCanonCamcoder = new Product()
            {
                Name = "Canon VIXIA HF100 Camcorder",
                ShortDescription = "12x optical zoom; SuperRange Optical Image Stabilizer",
                Description = "<p>From Canon's long history of optical excellence, advanced image processing, superb performance and technological innovation in photographic and broadcast television cameras comes the latest in high definition camcorders. <br /><br />Now, with the light, compact Canon VIXIA HF100, you can have stunning AVCHD (Advanced Video Codec High Definition) format recording with the ease and numerous benefits of Flash Memory. It's used in some of the world's most innovative electronic products such as laptop computers, MP3 players, PDAs and cell phones. <br /><br />Add to that the VIXIA HF100's Canon Exclusive features such as our own 3.3 Megapixel Full HD CMOS sensor and advanced DIGIC DV II Image Processor, SuperRange Optical Image Stabilization, Instant Auto Focus, our 2.7-inch Widescreen Multi-Angle Vivid LCD and the Genuine Canon 12x HD video zoom lens and you have a Flash Memory camcorder that's hard to beat.</p>",
                Price = 530M,
                Weight = 7,
                Length = 7,
                Width = 7,
                Height = 7,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow
            };
            allProducts.Add(productCanonCamcoder);
            productCanonCamcoder.ProductCategories.Add(new ProductCategory()
            {
                Category = _categoryRepository.Table.Single(c => c.Name == "Camera, photo"),
                ShowOrder = 1,
            });
            productCanonCamcoder.ProductPictures.Add(new ProductPicture()
            {
                Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_CanonCamcoder.jpeg"), "image/jpeg", true),
                ShowOrder = 1,
            });
            _productRepository.Insert(productCanonCamcoder);

            var productCompaq = new Product()
            {
                Name = "Compaq Presario SR1519X Pentium 4 Desktop PC with CDRW",
                ShortDescription = "Compaq Presario Desktop PC",
                Description = "<p>Compaq Presario PCs give you solid performance, ease of use, and deliver just what you need so you can do more with less effort. Whether you are e-mailing family, balancing your online checkbook or creating school projects, the Presario is the right PC for you.</p>",
                Price = 500M,
                Weight = 7,
                Length = 7,
                Width = 7,
                Height = 7,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow
            };
            allProducts.Add(productCompaq);
            productCompaq.ProductCategories.Add(new ProductCategory()
            {
                Category = _categoryRepository.Table.Single(c => c.Name == "Desktops"),
                ShowOrder = 1,
            });
            productCompaq.ProductPictures.Add(new ProductPicture()
            {
                Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_Compaq.jpeg"), "image/jpeg", true),
                ShowOrder = 1,
            });
            _productRepository.Insert(productCompaq);

            var productCookingForTwo = new Product()
            {
                Name = "Cooking for Two",
                ShortDescription = "More Than 200 Foolproof Recipes for Weeknights and Special Occasions (Hardcover)",
                Description = "<p>Hardcover: 352 pages<br />Publisher: America's Test Kitchen (May 2009)<br />Language: English<br />ISBN-10: 1933615435<br />ISBN-13: 978-1933615431</p>",
                Price = 19M,
                OldPrice = 27M,
                Weight = 2,
                Length = 2,
                Width = 2,
                Height = 2,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow
            };
            allProducts.Add(productCookingForTwo);
            productCookingForTwo.ProductCategories.Add(new ProductCategory()
            {
                Category = _categoryRepository.Table.Single(c => c.Name == "Books"),
                ShowOrder = 1,
            });
            productCookingForTwo.ProductPictures.Add(new ProductPicture()
            {
                Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_CookingForTwo.jpeg"), "image/jpeg", true),
                ShowOrder = 1,
            });
            _productRepository.Insert(productCookingForTwo);

            var productCorel = new Product()
            {
                Name = "Corel Paint Shop Pro Photo X2",
                ShortDescription = "The ideal choice for any aspiring photographer's digital darkroom",
                Description = "<p>Corel Paint Shop Pro Photo X2 is the ideal choice for any aspiring photographer's digital darkroom. Fix brightness, color, and photo flaws in a few clicks; use precision editing tools to create the picture you want; give photos a unique, exciting look using hundreds of special effects, and much more! Plus, the NEW one-of-a-kind Express Lab helps you quickly view and fix dozens of photos in the time it used to take to edit a few. Paint Shop Pro Photo X2 even includes a built-in Learning Center to help you get started, it's the easiest way to get professional-looking photos - fast!</p>",
                Price = 65M,
                Weight = 2,
                Length = 2,
                Width = 2,
                Height = 3,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow
            };
            allProducts.Add(productCorel);
            productCorel.ProductCategories.Add(new ProductCategory()
            {
                Category = _categoryRepository.Table.Single(c => c.Name == "Software & Games"),
                ShowOrder = 1,
            });
            productCorel.ProductPictures.Add(new ProductPicture()
            {
                Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_Corel.jpeg"), "image/jpeg", true),
                ShowOrder = 1,
            });
            _productRepository.Insert(productCorel);

            var productCustomTShirt = new Product()
            {
                Name = "Custom T-Shirt",
                ShortDescription = "T-Shirt - Add Your Content",
                Description = "<p>Comfort comes in all shapes and forms, yet this tee out does it all. Rising above the rest, our classic cotton crew provides the simple practicality you need to make it through the day. Tag-free, relaxed fit wears well under dress shirts or stands alone in laid-back style. Reinforced collar and lightweight feel give way to long-lasting shape and breathability. One less thing to worry about, rely on this tee to provide comfort and ease with every wear.</p>",
                Price = 15M,
                Weight = 4,
                Length = 3,
                Width = 3,
                Height = 3,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow
            };
            allProducts.Add(productCustomTShirt);
            productCustomTShirt.ProductCategories.Add(new ProductCategory()
            {
                Category = _categoryRepository.Table.Single(c => c.Name == "Apparel & Shoes"),
                ShowOrder = 1,
            });
            productCustomTShirt.ProductPictures.Add(new ProductPicture()
            {
                Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_CustomTShirt.jpeg"), "image/jpeg", true),
                ShowOrder = 1,
            });
            _productRepository.Insert(productCustomTShirt);

            var productDiamondEarrings = new Product()
            {
                Name = "Diamond Pave Earrings",
                ShortDescription = "1/2 Carat (ctw) in White Gold",
                Description = "<p>Perfect for both a professional look as well as perhaps something more sensual, these 10 karat white gold huggie earrings boast 86 sparkling round diamonds set in a pave arrangement that total 1/2 carat (ctw).</p>",
                Price = 569M,
                Weight = 2,
                Length = 2,
                Width = 2,
                Height = 2,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow
            };
            allProducts.Add(productDiamondEarrings);
            productDiamondEarrings.ProductCategories.Add(new ProductCategory()
            {
                Category = _categoryRepository.Table.Single(c => c.Name == "Jewelry"),
                ShowOrder = 1,
            });
            productDiamondEarrings.ProductPictures.Add(new ProductPicture()
            {
                Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_DiamondEarrings.jpg"), "image/pjpeg", true),
                ShowOrder = 1,
            });
            _productRepository.Insert(productDiamondEarrings);

            var productDiamondBracelet = new Product()
            {
                Name = "Diamond Tennis Bracelet",
                ShortDescription = "1.0 Carat (ctw) in White Gold",
                Description = "<p>Jazz up any outfit with this classic diamond tennis bracelet. This piece has one full carat of diamonds uniquely set in brilliant 10 karat white gold.</p>",
                Price = 360M,
                Weight = 2,
                Length = 2,
                Width = 2,
                Height = 2,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow
            };
            allProducts.Add(productDiamondBracelet);
            productDiamondBracelet.ProductCategories.Add(new ProductCategory()
            {
                Category = _categoryRepository.Table.Single(c => c.Name == "Jewelry"),
                ShowOrder = 1,
            });
            productDiamondBracelet.ProductPictures.Add(new ProductPicture()
            {
                Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_DiamondBracelet_1.jpg"), "image/pjpeg", true),
                ShowOrder = 1,
            });
            productDiamondBracelet.ProductPictures.Add(new ProductPicture()
            {
                Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_DiamondBracelet_2.jpg"), "image/pjpeg", true),
                ShowOrder = 2,
            });
            _productRepository.Insert(productDiamondBracelet);

            var productEatingWell = new Product()
            {
                Name = "EatingWell in Season",
                ShortDescription = "A Farmers' Market Cookbook (Hardcover)",
                Description = "<p>Trying to get big chocolate flavor into a crisp holiday cookie is no easy feat. Any decent baker can get a soft, chewy cookie to scream &ldquo;chocolate,&rdquo; but a dough that can withstand a rolling pin and cookie cutters simply can&rsquo;t be too soft. Most chocolate butter cookies skimp on the gooey chocolate and their chocolate flavor is quite modest.</p>",
                Price = 51M,
                OldPrice = 67M,
                Weight = 2,
                Length = 2,
                Width = 2,
                Height = 2,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow
            };
            allProducts.Add(productEatingWell);
            productEatingWell.ProductCategories.Add(new ProductCategory()
            {
                Category = _categoryRepository.Table.Single(c => c.Name == "Books"),
                ShowOrder = 1,
            });
            productEatingWell.ProductPictures.Add(new ProductPicture()
            {
                Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_EatingWell.jpeg"), "image/jpeg", true),
                ShowOrder = 1,
            });
            _productRepository.Insert(productEatingWell);

            var productEtnies = new Product()
            {
                Name = "etnies Men's Digit Sneaker",
                ShortDescription = "This sleek shoe has all you need--from the padded tongue and collar and internal EVA midsole, to the STI Level 2 cushioning for impact absorption and stability.",
                Description = "<p>Established in 1986, etnies is the first skateboarder-owned and skateboarder-operated global action sports footwear and apparel company. etnies not only pushed the envelope by creating the first pro model skate shoe, but it pioneered technological advances and changed the face of skateboard footwear forever. Today, etnies' vision is to remain the leading action sports company committed to creating functional products that provide the most style, comfort, durability and protection possible. etnies stays true to its roots by sponsoring a world-class team of skateboarding, surfing, snowboarding, moto-x, and BMX athletes and continues its dedication by giving back to each of these communities.</p>",
                Price = 17.56M,
                Weight = 2,
                Length = 2,
                Width = 2,
                Height = 2,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow
            };
            allProducts.Add(productEtnies);
            productEtnies.ProductCategories.Add(new ProductCategory()
            {
                Category = _categoryRepository.Table.Single(c => c.Name == "Apparel & Shoes"),
                ShowOrder = 1,
            });
            productEtnies.ProductPictures.Add(new ProductPicture()
            {
                Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_Etnies.jpg"), "image/pjpeg", true),
                ShowOrder = 1,
            });
            _productRepository.Insert(productEtnies);

            var productLeatherHandbag = new Product()
            {
                Name = "Genuine Leather Handbag with Cell Phone Holder & Many Pockets",
                ShortDescription = "Classic Leather Handbag",
                Description = "<p>This fine leather handbag will quickly become your favorite bag. It has a zipper organizer on the front that includes a notepad pocket, pen holder, credit card slots and zipper pocket divider. On top of this is a zipper pocket and another flap closure pocket. The main compartment is fully lined and includes a side zipper pocket. On the back is another zipper pocket. And don't forget the convenient built in cell phone holder on the side! The long strap is fully adjustable so you can wear it crossbody or over the shoulder. This is a very well-made, quality leather bag that is not too big, but not too small.</p>",
                Price = 35M,
                Weight = 2,
                Length = 2,
                Width = 2,
                Height = 2,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow
            };
            allProducts.Add(productLeatherHandbag);
            productLeatherHandbag.ProductCategories.Add(new ProductCategory()
            {
                Category = _categoryRepository.Table.Single(c => c.Name == "Apparel & Shoes"),
                ShowOrder = 1,
            });
            productLeatherHandbag.ProductPictures.Add(new ProductPicture()
            {
                Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_LeatherHandbag_1.jpg"), "image/pjpeg", true),
                ShowOrder = 1,
            });
            productLeatherHandbag.ProductPictures.Add(new ProductPicture()
            {
                Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_LeatherHandbag_2.jpg"), "image/pjpeg", true),
                ShowOrder = 2,
            });
            _productRepository.Insert(productLeatherHandbag);

            var productHp506 = new Product()
            {
                Name = "HP IQ506 TouchSmart Desktop PC",
                ShortDescription = "",
                Description = "<p>Redesigned with a next-generation, touch-enabled 22-inch high-definition LCD screen, the HP TouchSmart IQ506 all-in-one desktop PC is designed to fit wherever life happens: in the kitchen, family room, or living room. With one touch you can check the weather, download your e-mail, or watch your favorite TV show. It's also designed to maximize energy, with a power-saving Intel Core 2 Duo processor and advanced power management technology, as well as material efficiency--right down to the packaging. It has a sleek piano black design with elegant espresso side-panel highlights, and the HP Ambient Light lets you set a mood--or see your keyboard in the dark.</p>",
                Price = 1199M,
                Weight = 7,
                Length = 7,
                Width = 7,
                Height = 7,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow
            };
            allProducts.Add(productHp506);
            productHp506.ProductCategories.Add(new ProductCategory()
            {
                Category = _categoryRepository.Table.Single(c => c.Name == "Desktops"),
                ShowOrder = 1,
            });
            productHp506.ProductPictures.Add(new ProductPicture()
            {
                Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_Hp506.jpeg"), "image/jpeg", true),
                ShowOrder = 1,
            });
            _productRepository.Insert(productHp506);

            var productHpPavilion1 = new Product()
            {
                Name = "HP Pavilion Artist Edition DV2890NR 14.1-inch Laptop",
                ShortDescription = "Unique Asian-influenced HP imprint wraps the laptop both inside and out",
                Description = "<p>Optimize your mobility with a BrightView 14.1-inch display that has the same viewable area as a 15.4-inch screen--in a notebook that weighs a pound less. Encouraging more direct interaction, the backlit media control panel responds to the touch or sweep of a finger. Control settings for audio and video playback from up to 10 feet away with the included HP remote, then store it conveniently in the PC card slot. Enjoy movies or music in seconds with the external DVD or music buttons to launch HP QuickPlay (which bypasses the boot process).</p><p>It's powered by the 1.83 GHz Intel Core 2 Duo T5550 processor, which provides an optimized, multithreaded architecture for improved gaming and multitasking performance, as well as excellent battery management. It also includes Intel's 4965 AGN wireless LAN, which will connect to draft 802.11n routers and offers compatibility with 802.11a/b/g networks as well. It also features a 250 GB hard drive, 3 GB of installed RAM (4 GB maximum), LighScribe dual-layer DVD&plusmn;R burner, HDMI port for connecting to an HDTV, and Nvidia GeForce Go 8400M GS video/graphics card with up to 1407 MB of total allocated video memory (128 MB dedicated). It also includes an integrated Webcam in the LCD's bezel and an omnidirectional microphone for easy video chats.</p>",
                Price = 1590M,
                Weight = 7,
                Length = 7,
                Width = 7,
                Height = 7,
                ShowOnHomePage = true,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow
            };
            allProducts.Add(productHpPavilion1);
            productHpPavilion1.ProductCategories.Add(new ProductCategory()
            {
                Category = _categoryRepository.Table.Single(c => c.Name == "Notebooks"),
                ShowOrder = 1,
            });
            productHpPavilion1.ProductPictures.Add(new ProductPicture()
            {
                Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_HpPavilion1.jpeg"), "image/jpeg", true),
                ShowOrder = 1,
            });
            _productRepository.Insert(productHpPavilion1);

            var productHpPavilion2 = new Product()
            {
                Name = "HP Pavilion Elite M9150F Desktop PC",
                ShortDescription = "Top-of-the-line multimedia desktop featuring 2.4 GHz Intel Core 2 Quad Processor Q6600 with four lightning fast execution cores",
                Description = "<p>The updated chassis with sleek piano black paneling and components is far from the most significant improvements in the multimedia powerhouse HP Pavilion Elite m9150f desktop PC. It's powered by Intel's newest processor--the 2.4 GHz Intel Core 2 Quad Q6600--which delivers four complete execution cores within a single processor for unprecedented performance and responsiveness in multi-threaded and multi-tasking business/home environments. You can also go wireless and clutter-free with wireless keyboard, mouse, and remote control, and it includes the next step in Wi-Fi networking with a 54g wireless LAN (802.11b/g).</p>",
                Price = 1350M,
                Weight = 7,
                Length = 7,
                Width = 7,
                Height = 7,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow
            };
            allProducts.Add(productHpPavilion2);
            productHpPavilion2.ProductCategories.Add(new ProductCategory()
            {
                Category = _categoryRepository.Table.Single(c => c.Name == "Desktops"),
                ShowOrder = 1,
            });
            productHpPavilion2.ProductPictures.Add(new ProductPicture()
            {
                Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_HpPavilion2_1.jpeg"), "image/jpeg", true),
                ShowOrder = 1,
            });
            productHpPavilion2.ProductPictures.Add(new ProductPicture()
            {
                Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_HpPavilion2_2.jpeg"), "image/jpeg", true),
                ShowOrder = 2,
            });
            _productRepository.Insert(productHpPavilion2);

            var productHpPavilion3 = new Product()
            {
                Name = "HP Pavilion G60-230US 16.0-Inch Laptop",
                ShortDescription = "Streamlined multimedia laptop with 16-inch screen for basic computing, entertainment and online communication",
                Description = "<p>Chat face to face, or take pictures and video clips with the webcam and integrated digital microphone. Play games and enhance multimedia with the Intel GMA 4500M with up to 1309 MB of total available graphics memory. And enjoy movies or music in seconds with the external DVD or music buttons to launch HP QuickPlay (which bypasses the boot process).  It offers dual-core productivity from its 2.0 GHz Intel Pentium T4200 processor for excellent multitasking. Other features include a 320 GB hard drive, 3 GB of installed RAM (4 GB maximum capacity), dual-layer DVD&plusmn;RW drive (which also burns CDs), quad-mode Wi-Fi (802.11a/b/g/n), 5-in-1 memory card reader, and pre-installed Windows Vista Home Premium (SP1).</p>",
                Price = 1460M,
                Weight = 7,
                Length = 7,
                Width = 7,
                Height = 7,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow
            };
            allProducts.Add(productHpPavilion3);
            productHpPavilion3.ProductCategories.Add(new ProductCategory()
            {
                Category = _categoryRepository.Table.Single(c => c.Name == "Notebooks"),
                ShowOrder = 1,
            });
            productHpPavilion3.ProductPictures.Add(new ProductPicture()
            {
                Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_HpPavilion3.jpeg"), "image/jpeg", true),
                ShowOrder = 1,
            });
            _productRepository.Insert(productHpPavilion3);

            var productHat = new Product()
            {
                Name = "Indiana Jones® Shapeable Wool Hat",
                ShortDescription = "Wear some adventure with the same hat Indiana Jones&reg; wears in his movies.",
                Description = "<p>Wear some adventure with the same hat Indiana Jones&reg; wears in his movies. Easy to shape to fit your personal style. Wool. Import. Please Note - Due to new UPS shipping rules and the size of the box, if you choose to expedite your hat order (UPS 3-day, 2-day or Overnight), an additional non-refundable $20 shipping charge per hat will be added at the time your order is processed.</p>",
                Price = 30M,
                Weight = 2,
                Length = 2,
                Width = 2,
                Height = 2,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow
            };
            allProducts.Add(productHat);
            productHat.ProductCategories.Add(new ProductCategory()
            {
                Category = _categoryRepository.Table.Single(c => c.Name == "Apparel & Shoes"),
                ShowOrder = 1,
            });
            productHat.ProductPictures.Add(new ProductPicture()
            {
                Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_hat.jpg"), "image/pjpeg", true),
                ShowOrder = 1,
            });
            _productRepository.Insert(productHat);

            var productKensington = new Product()
            {
                Name = "Kensington 33117 International All-in-One Travel Plug Adapter",
                ShortDescription = "Includes plug adapters for use in more than 150 countries",
                Description = "<p>The Kensington 33117 Travel Plug Adapter is a pocket-sized power adapter for go-anywhere convenience. This all-in-one unit provides plug adapters for use in more than 150 countries, so you never need to be at a loss for power again. The Kensington 33117 is easy to use, with slide-out power plugs that ensure you won't lose any vital pieces, in a compact, self-contained unit that eliminates any hassles. This all-in-one plug adapts power outlets for laptops, chargers, and similar devices, and features a safety release button and built-in fuse to ensure safe operation. The Kensington 33117 does not reduce or convert electrical voltage, is suitable for most consumer electronics ranging from 110-volts to Mac 275-watts, to 220-volts to Mac 550-watts. Backed by Kensington's one-year warranty, this unit weighs 0.5, and measures 1.875 x 2 x 2.25 inches (WxDxH). Please note that this adapter is not designed for use with high-watt devices such as hairdryers and irons, so users should check electronic device specifications before using.</p>",
                Price = 35M,
                Weight = 7,
                Length = 7,
                Width = 7,
                Height = 7,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow
            };
            allProducts.Add(productKensington);
            productKensington.ProductCategories.Add(new ProductCategory()
            {
                Category = _categoryRepository.Table.Single(c => c.Name == "Accessories"),
                ShowOrder = 1,
            });
            productKensington.ProductPictures.Add(new ProductPicture()
            {
                Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_Kensington.jpeg"), "image/jpeg", true),
                ShowOrder = 1,
            });
            _productRepository.Insert(productKensington);

            var productLeviJeans = new Product()
            {
                Name = "Levi's Skinny 511 Jeans",
                ShortDescription = "Levi's Faded Black Skinny 511 Jeans ",
                Description = "",
                Price = 43.5M,
                OldPrice = 55M,
                Weight = 2,
                Length = 2,
                Width = 2,
                Height = 2,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow
            };
            allProducts.Add(productLeviJeans);
            productLeviJeans.ProductCategories.Add(new ProductCategory()
            {
                Category = _categoryRepository.Table.Single(c => c.Name == "Apparel & Shoes"),
                ShowOrder = 1,
            });
            productLeviJeans.ProductPictures.Add(new ProductPicture()
            {
                Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_LeviJeans_1.jpg"), "image/pjpeg", true),
                ShowOrder = 1,
            });
            productLeviJeans.ProductPictures.Add(new ProductPicture()
            {
                Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_LeviJeans_2.jpg"), "image/pjpeg", true),
                ShowOrder = 2,
            });
            _productRepository.Insert(productLeviJeans);

            var productBaseball = new Product()
            {
                Name = "Major League Baseball 2K9",
                ShortDescription = "Take charge of your franchise and enjoy the all-new MLB.com presentation style",
                Description = "<p>Major League Baseball 2K9 captures the essence of baseball down to some of the most minute, player- specific details including batting stances, pitching windups and signature swings. 2K Sports has gone above and beyond the call of duty to deliver this in true major league fashion. Additionally, gameplay enhancements in pitching, batting, fielding and base running promise this year's installment to be user-friendly and enjoyable for rookies or veterans. New commentary and presentation provide the icing to this ultimate baseball experience. If you really want to Play Ball this is the game for you.</p>",
                Price = 14.99M,
                Weight = 7,
                Length = 7,
                Width = 7,
                Height = 7,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow
            };
            allProducts.Add(productBaseball);
            productBaseball.ProductCategories.Add(new ProductCategory()
            {
                Category = _categoryRepository.Table.Single(c => c.Name == "Software & Games"),
                ShowOrder = 1,
            });
            productBaseball.ProductPictures.Add(new ProductPicture()
            {
                Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_Baseball.jpeg"), "image/jpeg", true),
                ShowOrder = 1,
            });
            _productRepository.Insert(productBaseball);

            var productMedalOfHonor = new Product()
            {
                Name = "Medal of Honor - Limited Edition (Xbox 360)",
                ShortDescription = "One of the great pioneers in military simulations returns to gaming as the Medal of Honor series depicts modern warfare for the first time, with a harrowing tour of duty in current day Afghanistan.",
                Description = "You'll take control of both ordinary U.S. Army Rangers and Tier 1 Elite Ops Special Forces as you fight enemy insurgents in the most dangerous theatre of war of the modern age. The intense first person combat has been created with input from U.S. military consultants and based on real-life descriptions from veteran soldiers. This allows you to use genuine military tactics and advanced technology including combat drones and targeted air strikes.",
                Price = 37M,
                Weight = 7,
                Length = 7,
                Width = 7,
                Height = 7,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow
            };
            allProducts.Add(productMedalOfHonor);
            productMedalOfHonor.ProductCategories.Add(new ProductCategory()
            {
                Category = _categoryRepository.Table.Single(c => c.Name == "Software & Games"),
                ShowOrder = 1,
            });
            productMedalOfHonor.ProductPictures.Add(new ProductPicture()
            {
                Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_MedalOfHonor.jpeg"), "image/jpeg", true),
                ShowOrder = 1,
            });
            _productRepository.Insert(productMedalOfHonor);





            var productMouse = new Product()
            {
                Name = "Microsoft Bluetooth Notebook Mouse 5000 Mac/Windows",
                ShortDescription = "Enjoy reliable, transceiver-free wireless connection to your PC with Bluetooth Technology",
                Description = "<p>Enjoy wireless freedom with the Microsoft&reg; Bluetooth&reg; Notebook Mouse 5000 &mdash; no transceiver to connect or lose! Keep USB ports free for other devices. And, take it with you in a convenient carrying case (included)</p>",
                Price = 37M,
                Weight = 7,
                Length = 7,
                Width = 7,
                Height = 7,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow
            };
            allProducts.Add(productMouse);
            productMouse.ProductCategories.Add(new ProductCategory()
            {
                Category = _categoryRepository.Table.Single(c => c.Name == "Accessories"),
                ShowOrder = 1,
            });
            productMouse.ProductPictures.Add(new ProductPicture()
            {
                Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_Mouse.jpeg"), "image/jpeg", true),
                ShowOrder = 1,
            });
            _productRepository.Insert(productMouse);

            var productGolfBelt = new Product()
            {
                Name = "NIKE Golf Casual Belt",
                ShortDescription = "NIKE Golf Casual Belt is a great look for in the clubhouse after a round of golf.",
                Description = "<p>NIKE Golf Casual Belt is a great look for in the clubhouse after a round of golf. The belt strap is made of full grain oil tanned leather. The buckle is made of antique brushed metal with an embossed Swoosh design on it. This belt features an English beveled edge with rivets on the tab and tip of the 38mm wide strap. Size: 32; Color: Black.</p>",
                Price = 45M,
                Weight = 7,
                Length = 7,
                Width = 7,
                Height = 7,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow
            };
            allProducts.Add(productGolfBelt);
            productGolfBelt.ProductCategories.Add(new ProductCategory()
            {
                Category = _categoryRepository.Table.Single(c => c.Name == "Apparel & Shoes"),
                ShowOrder = 1,
            });
            productGolfBelt.ProductPictures.Add(new ProductPicture()
            {
                Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_GolfBelt.jpeg"), "image/jpeg", true),
                ShowOrder = 1,
            });
            _productRepository.Insert(productGolfBelt);

            var productPanasonic = new Product()
            {
                Name = "Panasonic HDC-SDT750K, High Definition 3D Camcorder",
                ShortDescription = "World's first 3D Shooting Camcorder",
                Description = "<p>Unlike previous 3D images that required complex, professional equipment to create, now you can shoot your own. Simply attach the 3D Conversion Lens to the SDT750 for quick and easy 3D shooting. And because the SDT750 features the Advanced 3MOS System, which has gained worldwide popularity, colors are vivid and 3D images are extremely realistic. Let the SDT750 save precious moments for you in true-to-life images.</p>",
                Price = 1300M,
                Weight = 7,
                Length = 7,
                Width = 7,
                Height = 7,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow
            };
            allProducts.Add(productPanasonic);
            productPanasonic.ProductCategories.Add(new ProductCategory()
            {
                Category = _categoryRepository.Table.Single(c => c.Name == "Camera, photo"),
                ShowOrder = 1,
            });
            productPanasonic.ProductPictures.Add(new ProductPicture()
            {
                Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_productPanasonic.jpeg"), "image/jpeg", true),
                ShowOrder = 1,
            });
            _productRepository.Insert(productPanasonic);
            
            var productSunglasses = new Product()
            {
                Name = "Ray Ban Aviator Sunglasses RB 3025",
                ShortDescription = "Aviator sunglasses are one of the first widely popularized styles of modern day sunwear.",
                Description = "<p>Since 1937, Ray-Ban can genuinely claim the title as the world's leading sunglasses and optical eyewear brand. Combining the best of fashion and sports performance, the Ray-Ban line of Sunglasses delivers a truly classic style that will have you looking great today and for years to come.</p>",
                Price = 25M,
                Weight = 7,
                Length = 7,
                Width = 7,
                Height = 7,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow
            };
            allProducts.Add(productSunglasses);
            productSunglasses.ProductCategories.Add(new ProductCategory()
            {
                Category = _categoryRepository.Table.Single(c => c.Name == "Apparel & Shoes"),
                ShowOrder = 1,
            });
            productSunglasses.ProductPictures.Add(new ProductPicture()
            {
                Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_Sunglasses.jpg"), "image/pjpeg", true),
                ShowOrder = 1,
            });
            _productRepository.Insert(productSunglasses);

            var productSamsungPhone = new Product()
            {
                Name = "Samsung Rugby A837 Phone, Black (AT&T)",
                ShortDescription = "Ruggedized 3G handset in black great for outdoor workforces",
                Description = "<p>Ideal for on-site field services, the ruggedized Samsung Rugby for AT&amp;T can take just about anything you can throw at it. This highly durable handset is certified to Military Standard MIL-STD 810F standards that's perfect for users like construction foremen and landscape designers. In addition to access to AT&amp;T Navigation turn-by-turn direction service, the Rugby also features compatibility with Push to Talk communication, Enterprise Paging, and AT&amp;T's breakthrough Video Share calling services. This quad-band GSM phone runs on AT&amp;T's dual-band 3G (HSDPA/UMTS) network, for fast downloads and seamless video calls. It also offers a 1.3-megapixel camera, microSD memory expansion to 8 GB, Bluetooth for handsfree communication and stereo music streaming, access to personal email and instant messaging, and up to 5 hours of talk time.</p>",
                Price = 100M,
                Weight = 7,
                Length = 7,
                Width = 7,
                Height = 7,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow
            };
            allProducts.Add(productSamsungPhone);
            productSamsungPhone.ProductCategories.Add(new ProductCategory()
            {
                Category = _categoryRepository.Table.Single(c => c.Name == "Cell phones"),
                ShowOrder = 1,
            });
            productSamsungPhone.ProductPictures.Add(new ProductPicture()
            {
                Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_SamsungPhone_1.jpeg"), "image/jpeg", true),
                ShowOrder = 1,
            });
            productSamsungPhone.ProductPictures.Add(new ProductPicture()
            {
                Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_SamsungPhone_2.jpeg"), "image/jpeg", true),
                ShowOrder = 2,
            });
            _productRepository.Insert(productSamsungPhone);

            var productSonyCamcoder = new Product()
            {
                Name = "Sony DCR-SR85 1MP 60GB Hard Drive Handycam Camcorder",
                ShortDescription = "Capture video to hard disk drive; 60 GB storage",
                Description = "<p>You&rsquo;ll never miss a moment because of switching tapes or discs with the DCR-SR85. Its built-in 60GB hard disk drive offers plenty of storage as you zero in on your subjects with the professional-quality Carl Zeiss Vario-Tessar lens and a powerful 25x optical/2000x digital zoom. Compose shots using the 2.7-inch wide (16:9) touch-panel LCD display, and maintain total control and clarity with the Super SteadyShot image stabilization system. Hybrid recording technology even gives you the choice to record video to either the internal hard disk drive or removable Memory Stick Pro Duo media.</p>",
                Price = 349M,
                Weight = 7,
                Length = 7,
                Width = 7,
                Height = 7,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow
            };
            allProducts.Add(productSonyCamcoder);
            productSonyCamcoder.ProductCategories.Add(new ProductCategory()
            {
                Category = _categoryRepository.Table.Single(c => c.Name == "Camera, photo"),
                ShowOrder = 1,
            });
            productSonyCamcoder.ProductPictures.Add(new ProductPicture()
            {
                Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_SonyCamcoder.jpeg"), "image/jpeg", true),
                ShowOrder = 1,
            });
            _productRepository.Insert(productSonyCamcoder);

            var productBestSkilletRecipes = new Product()
            {
                Name = "The Best Skillet Recipes",
                ShortDescription = "What's the Best Way to Make Lasagna With Rich, Meaty Flavor, Chunks of Tomato, and Gooey Cheese, Without Ever Turning on the Oven or Boiling a Pot of (Hardcover)",
                Description = "<p>In this latest addition of the Best Recipe Classic series, <em>Cooks Illustrated</em> editor Christopher Kimball and his team of kitchen scientists celebrate the untold versatility of that ordinary workhorse, the 12-inch skillet. An indispensable tool for eggs, pan-seared meats and saut&eacute;ed vegetables, the skillet can also be used for stovetop-to-oven dishes such as All-American Mini Meatloaves; layered dishes such as tamale pie and Tuscan bean casserole; and even desserts such as hot fudge pudding cake. In the trademark style of other America's Test Kitchen publications, the cookbook contains plenty of variations on basic themes (you can make chicken and rice with peas and scallions, broccoli and cheddar, or coconut milk and pistachios); ingredient and equipment roundups; and helpful illustrations for preparing mango and stringing snowpeas. Yet the true strength of the series lies in the sheer thoughtfulness and detail of the recipes. Whether or not you properly appreciate your skillet, this book will at least teach you to wield it gracefully. <i>(Mar.)</i>   <br />Copyright &copy; Reed Business Information, a division of Reed Elsevier Inc. All rights reserved.</p>",
                Price = 24M,
                OldPrice = 35M,
                Weight = 2,
                Length = 2,
                Width = 2,
                Height = 2,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow
            };
            allProducts.Add(productBestSkilletRecipes);
            productBestSkilletRecipes.ProductCategories.Add(new ProductCategory()
            {
                Category = _categoryRepository.Table.Single(c => c.Name == "Books"),
                ShowOrder = 1,
            });
            productBestSkilletRecipes.ProductPictures.Add(new ProductPicture()
            {
                Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_BestSkilletRecipes.jpeg"), "image/jpeg", true),
                ShowOrder = 1,
            });
            _productRepository.Insert(productBestSkilletRecipes);

            var productSatellite = new Product()
            {
                Name = "Toshiba Satellite A305-S6908 15.4-Inch Laptop",
                ShortDescription = "Stylish, highly versatile laptop with 15.4-inch LCD, webcam integrated into bezel, and high-gloss finish",
                Description = "<p>It's powered by the 2.0 GHz Intel Core 2 Duo T6400 processor, which boosts speed, reduces power requirements, and saves on battery life. It also offers a fast 800 MHz front-side bus speed and 2 MB L2 cache. It also includes Intel's 5100AGN wireless LAN, which will connect to draft 802.11n routers and offers compatibility with 802.11a/b/g networks as well. Other features include an enormous 250 GB hard drive,&nbsp;1 GB of installed RAM (max capacity), dual-layer DVD&plusmn;RW burner (with Labelflash disc printing), ExpressCard 54/34 slot, a combo USB/eSATA port, SPDIF digital audio output for surround sound, and a 5-in-1 memory card adapter.</p><p>This PC comes preinstalled with the 64-bit version of Microsoft Windows Vista Home Premium (SP1), which includes all of the Windows Media Center capabilities for turning your PC into an all-in-one home entertainment center. In addition to easily playing your DVD movies and managing your digital audio library, you'll be able to record and watch your favorite TV shows (even HDTV). Vista also integrates new search tools throughout the operating system, includes new parental control features, and offers new tools that can warn you of impending hardware failures</p>",
                Price = 1360M,
                Weight = 7,
                Length = 7,
                Width = 7,
                Height = 7,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow
            };
            allProducts.Add(productSatellite);
            productSatellite.ProductCategories.Add(new ProductCategory()
            {
                Category = _categoryRepository.Table.Single(c => c.Name == "Notebooks"),
                ShowOrder = 1,
            });
            productSatellite.ProductPictures.Add(new ProductPicture()
            {
                Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_Notebooks.jpeg"), "image/jpeg", true),
                ShowOrder = 1,
            });
            _productRepository.Insert(productSatellite);

            var productDenimShort = new Product()
            {
                Name = "V-Blue Juniors' Cuffed Denim Short with Rhinestones",
                ShortDescription = "Superior construction and reinforced seams",
                Description = "",
                Price = 10M,
                Weight = 2,
                Length = 2,
                Width = 2,
                Height = 2,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow
            };
            allProducts.Add(productDenimShort);
            productDenimShort.ProductCategories.Add(new ProductCategory()
            {
                Category = _categoryRepository.Table.Single(c => c.Name == "Apparel & Shoes"),
                ShowOrder = 1,
            });
            productDenimShort.ProductPictures.Add(new ProductPicture()
            {
                Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_DenimShort.jpg"), "image/pjpeg", true),
                ShowOrder = 1,
            });
            _productRepository.Insert(productDenimShort);

            var productEngagementRing = new Product()
            {
                Name = "Vintage Style Three Stone Diamond Engagement Ring",
                ShortDescription = "1.24 Carat (ctw) in 14K White Gold (Certified)",
                Description = "<p>Dazzle her with this gleaming 14 karat white gold vintage proposal. A ravishing collection of 11 decadent diamonds come together to invigorate a superbly ornate gold shank. Total diamond weight on this antique style engagement ring equals 1 1/4 carat (ctw). Item includes diamond certificate.</p>",
                Price = 2100M,
                Weight = 2,
                Length = 2,
                Width = 2,
                Height = 2,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow
            };
            allProducts.Add(productEngagementRing);
            productEngagementRing.ProductCategories.Add(new ProductCategory()
            {
                Category = _categoryRepository.Table.Single(c => c.Name == "Jewelry"),
                ShowOrder = 1,
            });
            productEngagementRing.ProductPictures.Add(new ProductPicture()
            {
                Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_EngagementRing_1.jpg"), "image/pjpeg", true),
                ShowOrder = 1,
            });
            productEngagementRing.ProductPictures.Add(new ProductPicture()
            {
                Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_EngagementRing_2.jpg"), "image/pjpeg", true),
                ShowOrder = 2,
            });
            _productRepository.Insert(productEngagementRing);

            var productWoW = new Product()
            {
                Name = "World of Warcraft: Wrath of the Lich King Expansion Pack",
                ShortDescription = "This expansion pack REQUIRES the original World of Warcraft game in order to run",
                Description = "<p>Fans of World of Warcraft, prepare for Blizzard Entertainment's next installment -- World of Warcraft: Wrath of King Lich. In this latest expansion, something is afoot in the cold, harsh northlands. The Lich King Arthas has set in motion events that could lead to the extinction of all life on Azeroth. The necromantic power of the plague and legions of undead armies threaten to sweep across the land. Only the mightiest heroes can oppose the Lich King and end his reign of terror.</p><p>This expansion adds a host of content to the already massive existing game world. Players will achieve soaring levels of power, explore Northrend (the vast icy continent of the Lich King), and battle high-level heroes to determine the ultimate fate of Azeroth. As you face the dangers of the frigid, harsh north, prepare to master the dark necromantic powers of the Death Night -- World of Warcraft's first Hero class. No longer servants of the Lich King, the Death Knights begin their new calling as experienced, formidable adversaries. Each is heavily armed, armored, and in possession of a deadly arsenal of forbidden magic.</p><p>If you have a World of Warcraft account with a character of at least level 55, you will be able to create a new level-55 Death Knight of any race (if on a PvP realm, the Death Knight must be the same faction as your existing character). And upon entering the new world, your Death Knight will begin to quest to level 80, gaining potent new abilities and talents along the way. This expansion allows for only one Death Knight per realm, per account.</p>",
                Price = 29.5M,
                Weight = 7,
                Length = 7,
                Width = 7,
                Height = 7,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow
            };
            allProducts.Add(productWoW);
            productWoW.ProductCategories.Add(new ProductCategory()
            {
                Category = _categoryRepository.Table.Single(c => c.Name == "Software & Games"),
                ShowOrder = 1,
            });
            productWoW.ProductPictures.Add(new ProductPicture()
            {
                Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_wow.jpeg"), "image/jpeg", true),
                ShowOrder = 1,
            });
            _productRepository.Insert(productWoW);

            var productSoccer = new Product()
            {
                Name = "World Wide Soccer Manager 2009",
                ShortDescription = "Worldwide Soccer Manager 2009 from Sega for the PC or Mac is an in-depth soccer management game",
                Description = "<p>Worldwide Soccer Manager 2009 from Sega for the PC or Mac is an in-depth soccer management game. At the helm, you'll enter the new season with a wide array of all-new features. The most impressive update is the first-time-ever, real-time 3D match engine with motion captured animations. With over 5,000 playable teams and every management decision in the palm of your hand, you'll love watching your matches and decisions unfold from multiple camera angles as you compete in leagues around the world and major international tournaments.</p><p>Watch your match in real-time, or use the Match Time Bar to fast-forward through sluggish minutes or rewind key moments in the game. With this customization at your fingertips you can also choose the information you'd like to see during the match, such as latest scores or player performance stats for the match.</p>",
                Price = 25.99M,
                Weight = 7,
                Length = 7,
                Width = 7,
                Height = 7,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow
            };
            allProducts.Add(productSoccer);
            productSoccer.ProductCategories.Add(new ProductCategory()
            {
                Category = _categoryRepository.Table.Single(c => c.Name == "Software & Games"),
                ShowOrder = 1,
            });
            productSoccer.ProductPictures.Add(new ProductPicture()
            {
                Picture = pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "product_Soccer.jpeg"), "image/jpeg", true),
                ShowOrder = 1,
            });
            _productRepository.Insert(productSoccer);


            //reviews
            var random = new Random();
            foreach (var product in allProducts)
            {
                //only 3 of 4 products will have reviews
                if (random.Next(4) == 3)
                    continue;

                //rating from 4 to 5
                var rating = random.Next(4, 6);
                product.ProductReviews.Add(new ProductReview()
                {
                    CustomerId = defaultCustomer.Id,
                    ProductId = product.Id,
                    Title = "Some sample review",
                    Content =string.Format("The {0} is good. It is priced just right.", product.Name),
                    //random (4 or 5)
                    Rating = rating,
                    CreatedOnUtc = DateTime.UtcNow
                });
                product.RatingSum = rating;
                product.TotalReviews = product.ProductReviews.Count;
                _productRepository.Update(product);
            }
        }

        #endregion
    }
}
