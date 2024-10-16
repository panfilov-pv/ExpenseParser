﻿using Autofac;
using BLL;
using BLL.Abstraction;
using BLL.XLSX;
using DAL;
using DAL.Abstraction;
using Microsoft.Extensions.Configuration;
using Model.Abstraction;
using OfficeOpenXml;
using Shared.Configuration;
using System.Reflection;

namespace ExpenseParser
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                IContainer containter = RegisterDependencies();

                if (containter == null)
                {
                    throw new ArgumentNullException(nameof(containter));
                }

                IWorker worker = containter.Resolve<IWorker>();

                worker.Work();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static IContainer RegisterDependencies()
        {
            var builder = new ContainerBuilder();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var configurationBuilder = new ConfigurationBuilder()
                    .SetBasePath(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..")))
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            IConfig config = new Config(configurationBuilder.Build());

            builder.RegisterInstance(config).As<IConfig>();

            builder.RegisterType<CardNumberResolver>().As<ICardNumberResolver>();
            builder.RegisterType<CategoryResolver>().As<ICategoryResolver>();

            builder.RegisterType<SberbankXlsxFileParser>().AsSelf();
            builder.RegisterType<TinkoffXlsxFileParser>().AsSelf();

            builder.RegisterType<XlsxFileSaver>().As<IFileSaver>();
            builder.RegisterType<DataPreparer>().As<IDataPreparer>();

            builder.RegisterType<Worker>().As<IWorker>();

            return builder.Build();
        }
    }
}