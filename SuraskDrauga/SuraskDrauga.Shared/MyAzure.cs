using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using System.Diagnostics;
using Windows.UI.Core;

namespace SuraskDrauga
{
    public class MyAzure
    {
        //public MobileServiceCollection<TableItem, TableItem> items;
        public List<TableItem> items;
        public IMobileServiceTable<TableItem> mainTable = App.MobileService.GetTable<TableItem>();
        public Task data, insert;//=GetData();
        private MapPage map;

        public MyAzure(MapPage map)
        {
            this.map = map;
            data = new Task(GetData);
        }

        public void startGetDataTask()
        {
            Debug.WriteLine("Tasko statusas: "+data.Status.ToString());
            if (data.Status!=TaskStatus.Running)
            {
                
                if (data.Status == TaskStatus.RanToCompletion)
                    data = new Task(GetData);
                data.Start();
            }
            data.Wait();
        }

        //public void StartInsertDataTask(TableItem item)
        //{
        //    if (insert.Status != TaskStatus.Running)
        //    {
        //        //data.S
        //        if (insert.Status == TaskStatus.RanToCompletion)
        //            insert = new Task(InsertData(item));
        //        data.Start();
        //    }
        //    insert.Wait();
        //}

        public async void GetData()
        {
            bool error = false;
            string str = "";
            //mainTable = App.MobileService.GetTable<TableItem>();
            //items = await mainTable.ToCollectionAsync();
            try
            {
                items = await mainTable.ToListAsync();
                //items = await mainTable.ToCollectionAsync();
            }
            catch (Exception e)
            {
                //Debug.WriteLine("Klaida updatinant" + e.ToString());
                error = true;
                str = e.ToString();
                items = null;
            }

            if (error) Debug.WriteLine("Klaida gaunant duomenis");
            //await map.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            //{
            //    map.Message("Klaida gaunant duomenis" + str);
            //});
        }

        public async Task InsertData(TableItem item)
        {
            bool error = false;
            string str = "";
            try
            {
                //await data;
                if (items.Any<TableItem>(c => c.Id == item.Id))
                    await mainTable.UpdateAsync(item);
                else await mainTable.InsertAsync(item);
                //items = await mainTable.ToListAsync();
            }
            catch (Exception e)
            {
                // Debug.WriteLine("Klaida uploadinant" + e.ToString());
                error = true;
                str = e.ToString();
            }
            if (error)
            await map.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                map.Message("Klaida uploadinant" + str);
            });
        }
    }
}
