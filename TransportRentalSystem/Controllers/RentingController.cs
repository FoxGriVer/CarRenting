using Ext.Net;
using Ext.Net.MVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TransportRentalSystem.ViewModels.JsonShare;
using TransportRentalSystem.ViewModels.RentingCar;
using DAL.Repository;
using Models.DataBaseModels;
using Models;
using System.Threading.Tasks;
using DAL;

namespace TransportRentalSystem.Controllers
{
    //[DirectController(AreaName = "Calendar_Overview", IDMode = DirectMethodProxyIDMode.None)]
    public class RentingController : Controller
    {
        ////Если есть идеи как можно сократить или оптимизировать данные репозитории, то нужно это сделать. Загружать все данные из БД каждый раз при загрузке календаря не очень.
        //JournOfAccountingRepository journOfAccountingRepository;
        //DriverRepository driverRepository;
        //StatusRepository statusRepository;
        //CarRepository carRepository;
        //DestinationPointRepository destinationPointRepository;

        private UnitOfWork unitOfWork;

        RentingViewModel rentingModel;

        /// <summary>
        /// Инициализируем в конструкторе все используемые в данном контроллере репозитории
        /// </summary>
        public RentingController()
        {
            //journOfAccountingRepository = new JournOfAccountingRepository();
            //driverRepository = new DriverRepository();
            //statusRepository = new StatusRepository();
            //carRepository = new CarRepository();
            //destinationPointRepository = new DestinationPointRepository();

            unitOfWork = new UnitOfWork();

            rentingModel = new RentingViewModel();
            InitializeAllEntites();
        }

        /// <summary>
        /// Отображает календарь
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View(InitializeEvents());
        }

        /// <summary>
        /// Данный метод пока не используется, но привязан к событию
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public ActionResult SubmitData(string data)
        {
            List<EventModel> events = JSON.Deserialize<List<EventModel>>(data);

            return new System.Web.Mvc.PartialViewResult
            {
                ViewName = "EventsViewer",
                ViewBag =
                {
                    Events = events
                }
            };
        }

        /// <summary>
        /// Метод для отображения всплывающего сообщения
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        [DirectMethod(Namespace = "CompanyX")]
        public ActionResult ShowMsg(string msg)
        {
            X.Msg.Notify("Message", msg).Show();

            return this.Direct();
        }

        /// <summary>
        /// Инициализирует все события на календаре
        /// </summary>
        /// <returns>Список событий</returns>
        public EventModelCollection InitializeEvents()
        {
            List<Journal> journalRecords = unitOfWork.JournsOfAccounting.GetManyObjects().ToList();
            EventModelCollection eventModelsCollection = new EventModelCollection();

            foreach (var record in journalRecords)
            {                
                RentingViewModel rentingModel = new RentingViewModel()
                {
                    EventId = record.JOURNAL_OF_ACCOUNTING_ID,
                    Title = record.FIO_DECLARANT,
                    StartDate = record.DEPARTURE_TIME,
                    EndDate = record.ARRIVAL_TIME,
                    IsAllDay = false,
                    Notes = record.COMMENTS,
                    Auto = record.CAR_ID,
                    Driver = record.DRIVER_ID,
                    PointSending = record.DESTINATION_POINT_SENDING_ID,
                    PointArrival = record.DESTINATION_POINT_ARRIVAL_ID,
                    Status = record.STATUS_ID,
                    CalendarId = record.STATUS_ID
                };
                eventModelsCollection.Add(rentingModel);
            }
            return eventModelsCollection;
        }

        /// <summary>
        /// Обрабатывает создание брони и перенаправляет на другую страницу
        /// </summary>
        /// <param name="inputStartTime">Сохраняет пришедшие в кеш</param>
        [HttpPost]
        public void SubmitDataFromCalendar(StartTimeViewModel inputStartTime)
        {
            TempData["TempCalendarData"] = inputStartTime;
        }

        /// <summary>
        /// Обрабатывает страницу бронирования
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult RentTransport()
        {
            rentingModel.CalendarViewModel = TempData["TempCalendarData"] as StartTimeViewModel;

            return View(rentingModel);
        }

        /// <summary>
        /// Обрабатывает страницу бронирования и создает запись в БД
        /// </summary>
        /// <param name="rentingModel"></param>
        /// <returns>Переход на календарь</returns>
        [HttpPost]
        public ActionResult RentTransport(RentingViewModel rentingModel)
        {
            Journal newRecord = new Journal()
            {
                FIO_DECLARANT = rentingModel.CalendarViewModel.Title,
                DRIVER_ID = rentingModel.Driver,
                CAR_ID = rentingModel.Auto,
                DESTINATION_POINT_SENDING_ID = rentingModel.PointSending,
                DESTINATION_POINT_ARRIVAL_ID = rentingModel.PointArrival,
                DEPARTURE_TIME = rentingModel.CalendarViewModel.StartDateTime,
                ARRIVAL_TIME = rentingModel.CalendarViewModel.EndDateTime,
                STATUS_ID = rentingModel.Status,
                COMMENTS = rentingModel.Notes
            };
            unitOfWork.JournsOfAccounting.InsertObject(newRecord);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Загружает данные из БД для их отображения в виде DropDownList 
        /// </summary>
        public void InitializeAllEntites()
        {
            rentingModel.Drivers = unitOfWork.Drivers.GetManyObjects().ToList();
            rentingModel.Autos = unitOfWork.Cars.GetManyObjects().ToList();
            rentingModel.TransportPoints = unitOfWork.DestinationPoints.GetManyObjects().ToList();
            rentingModel.Statuses = unitOfWork.Statuses.GetManyObjects().ToList();
        }

    }
}
