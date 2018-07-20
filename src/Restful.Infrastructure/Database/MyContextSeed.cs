using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using Restful.Core.Entities;
using Restful.Core.Entities.Milk;
using Restful.Core.Entities.RestDemo;

namespace Restful.Infrastructure.Database
{
    public class MyContextSeed
    {
        public static async Task SeedAsync(MyContext myContext,
                          ILoggerFactory loggerFactory, int retry = 0)
        {
            int retryForAvailability = retry;
            try
            {
                // TODO: Only run this if using a real database
                // myContext.Database.Migrate();

                if (!myContext.Countries.Any())
                {
                    myContext.Countries.AddRange(
                        new List<Country>{
                            new Country{
                                EnglishName = "China",
                                ChineseName = "中华人民共和国",
                                Abbreviation = "中国",
                                Cities = new List<City>
                                {
                                    new City{ Name = "北京", Description = "首都"},
                                    new City{ Name = "上海", Description = "魔都" },
                                    new City{ Name = "深圳" },
                                    new City{ Name = "杭州" },
                                    new City{ Name = "天津" }
                                }
                            },
                            new Country{
                                EnglishName = "USA",
                                ChineseName = "美利坚合众国",
                                Abbreviation = "美国",
                                Cities = new List<City>
                                {
                                    new City{ Name = "New York" },
                                    new City{ Name = "Chicago" },
                                    new City{ Name = "San Fransisco" },
                                    new City{ Name = "Los Angeles" },
                                    new City{ Name = "Miami" }
                                }
                            },
                            new Country{
                                EnglishName = "Finland",
                                ChineseName = "芬兰",
                                Abbreviation = "芬兰",
                                Cities = new List<City>
                                {
                                    new City{ Name = "Helsinki" },
                                    new City{ Name = "Espoo" },
                                    new City{ Name = "Tampere" }
                                }
                            },
                            new Country{
                                EnglishName = "UK",
                                ChineseName = "大不列颠及北爱尔兰联合王国",
                                Abbreviation = "英国",
                                Cities = new List<City>
                                {
                                    new City{ Name = "London" },
                                    new City{ Name = "Liverpool" },
                                    new City{ Name = "Manchester" },
                                    new City{ Name = "Birmingham" },
                                    new City{ Name = "Glasgow" }
                                }
                            },
                            new Country{
                                EnglishName = "Denmark",
                                ChineseName = "丹麦",
                                Abbreviation = "丹麦",
                                Cities = new List<City>
                                {
                                    new City{ Name = "Copenhagen " }
                                }
                            },
                            new Country{
                                EnglishName = "Norway",
                                ChineseName = "挪威",
                                Abbreviation = "挪威",
                                Cities = new List<City>
                                {
                                    new City{ Name = "Oslo" }
                                }
                            },
                            new Country{
                                EnglishName = "Sweden",
                                ChineseName = "瑞典",
                                Abbreviation = "瑞典",
                                Cities = new List<City>
                                {
                                    new City{ Name = "Stockholm" }
                                }
                            },
                            new Country{
                                EnglishName = "Germany",
                                ChineseName = "德意志联邦共和国",
                                Abbreviation = "德国",
                                Cities = new List<City>
                                {
                                    new City{ Name = "Berlin" }
                                }
                            },
                            new Country{
                                EnglishName = "Poland",
                                ChineseName = "波兰",
                                Abbreviation = "波兰",
                                Cities = new List<City>
                                {
                                    new City{ Name = "Warsaw" }
                                }
                            },
                            new Country{
                                EnglishName = "Switzerland",
                                ChineseName = "瑞士",
                                Abbreviation = "瑞士",
                                Cities = new List<City>
                                {
                                    new City{ Name = "Bern" }
                                }
                            },
                            new Country{
                                EnglishName = "Austria",
                                ChineseName = "奥地利",
                                Abbreviation = "奥地利",
                                Cities = new List<City>
                                {
                                    new City{ Name = "Vienna" }
                                }
                            }
                        }
                    );
                    await myContext.SaveChangesAsync();
                }

                if (!myContext.Products.Any())
                {
                    myContext.Products.AddRange(
                        new List<Product>{
                            new Product{
                                Name = "巴氏鲜牛奶",
                                PackingType = PackingType.PlasticBag,
                                OrderUnit = OrderUnit.ByOne,
                                QuantityPerBox = 20,
                                MinimumOrderUnitQuantity = 10,
                                UnitPrice = 2m
                            },
                            new Product{
                                Name = "巴氏珍品鲜牛奶",
                                PackingType = PackingType.PlasticBag,
                                OrderUnit = OrderUnit.ByOne,
                                QuantityPerBox = 20,
                                MinimumOrderUnitQuantity = 10,
                                UnitPrice = 2.5m
                            },
                            new Product{
                                Name = "酸牛奶",
                                PackingType = PackingType.PlasticBag,
                                OrderUnit = OrderUnit.ByOne,
                                QuantityPerBox = 20,
                                MinimumOrderUnitQuantity = 10,
                                UnitPrice = 2m
                            },
                            new Product{
                                Name = "珍品酸牛奶",
                                PackingType = PackingType.PlasticBag,
                                OrderUnit = OrderUnit.ByBox,
                                QuantityPerBox = 20,
                                MinimumOrderUnitQuantity = 1,
                                UnitPrice = 50m
                            },
                            new Product{
                                Name = "老北京王府酸牛奶",
                                PackingType = PackingType.PlasticCup,
                                OrderUnit = OrderUnit.ByBox,
                                QuantityPerBox = 30,
                                MinimumOrderUnitQuantity = 1,
                                UnitPrice = 90m,
                            },
                            new Product{
                                Name = "甜牛奶",
                                PackingType = PackingType.PlasticBottle,
                                OrderUnit = OrderUnit.ByBox,
                                QuantityPerBox = 15,
                                MinimumOrderUnitQuantity = 1,
                                UnitPrice = 75m,
                            },
                            new Product{
                                Name = "香蕉牛奶",
                                PackingType = PackingType.PlasticBottle,
                                OrderUnit = OrderUnit.ByBox,
                                QuantityPerBox = 15,
                                MinimumOrderUnitQuantity = 2,
                                UnitPrice = 72m,
                            },
                            new Product{
                                Name = "风味酸乳",
                                PackingType = PackingType.GlassBottle,
                                OrderUnit = OrderUnit.ByOne,
                                QuantityPerBox = 15,
                                MinimumOrderUnitQuantity = 1,
                                UnitPrice = 5m,
                            },
                            new Product{
                                Name = "木糖醇酸奶",
                                PackingType = PackingType.GlassBottle,
                                OrderUnit = OrderUnit.ByOne,
                                QuantityPerBox = 15,
                                MinimumOrderUnitQuantity = 1,
                                UnitPrice = 6m,
                            },
                            new Product{
                                Name = "乳酸菌酸奶",
                                PackingType = PackingType.GlassBottle,
                                OrderUnit = OrderUnit.ByBox,
                                QuantityPerBox = 10,
                                MinimumOrderUnitQuantity = 2,
                                UnitPrice = 75m,
                            }
                        }
                    );
                    await myContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                if (retryForAvailability < 10)
                {
                    retryForAvailability++;
                    var logger = loggerFactory.CreateLogger<MyContextSeed>();
                    logger.LogError(ex.Message);
                    await SeedAsync(myContext, loggerFactory, retryForAvailability);
                }
            }
        }
    }
}
