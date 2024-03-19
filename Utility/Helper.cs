using Microsoft.AspNetCore.Mvc.Rendering;
using MPA.Data;
using System;
using System.Collections.Generic;

namespace MPA.Utility
{
    public static class Helper
    {

        // LME Rule 
        public static string Spot = "Spot";
        public static string PWA = "Previous Week Average";
        public static string RCP = "RCP";
        public static string P5DA = "Previous 5 Days Average";

        public static string _30DD = "30 Days Delivery";

        public static string _15DD = "15 Days Delivery";
        public static string _7DD = "7 Days Delivery";
        public static string _5DD = "5 Days Dlivery";
        public static string _30DS = "30 Days Stamement";

        //Device
        public static string Computer = "Computer";
        public static string MobilePhone = "Mobile Phone";
        public static string Tablet = "Tablet";
        public static string OtherDevice = "Other";
        //Priority
        public static string Low = "Low";
        public static string Medium = "Medium";
        public static string High = "High";


        public static List<SelectListItem> GetLMERuleDropDown()
        {
            return new List<SelectListItem>
                {
                    new SelectListItem{Value=Helper.Spot,Text=Helper.Spot},
                    new SelectListItem{Value=Helper.PWA,Text=Helper.PWA},
                    new SelectListItem{Value=Helper.RCP,Text=Helper.RCP},
                    new SelectListItem{Value=Helper.P5DA,Text=Helper.P5DA},
                    new SelectListItem{Value=Helper._30DS,Text=Helper.P5DA},
                };
        }

        public static List<SelectListItem> GetPaymentTermsDropDown()
        {
            return new List<SelectListItem>
                {
                    new SelectListItem{Value="30",Text=Helper._30DD},
                    new SelectListItem{Value="15",Text=Helper._15DD},
                    new SelectListItem{Value="7",Text=Helper._7DD},
                    new SelectListItem{Value="5",Text=Helper._5DD},
                    new SelectListItem{Value="30",Text=Helper._30DS},
                };
        }

        public static List<SelectListItem> GetSystemDropDown()
        {
            return new List<SelectListItem>
            {
                // new SelectListItem{Value=Helper.CreditControl,Text=Helper.CreditControl},
                // new SelectListItem{Value=Helper.Indigent,Text=Helper.Indigent},
                // new SelectListItem{Value=Helper.Vatting,Text=Helper.Vatting},
                // new SelectListItem{Value=Helper.OtherSystem,Text=Helper.OtherSystem}
            };
        }

        public static List<SelectListItem> GetDeviceDropDown()
        {
            return new List<SelectListItem>
                {
                    new SelectListItem{Value=Helper.Computer,Text=Helper.Computer},
                    new SelectListItem{Value=Helper.MobilePhone,Text=Helper.MobilePhone},
                    new SelectListItem{Value=Helper.Tablet,Text=Helper.Tablet},
                    new SelectListItem{Value=Helper.OtherDevice,Text=Helper.OtherDevice}
                };
        }

        public static Status GetAlloyOrderStatus(decimal orderMass, decimal receivedOrderMass)
        {
            decimal per = (Math.Abs(orderMass - receivedOrderMass) / orderMass) * 100;
            if (per > 5)
            {
                if (orderMass > receivedOrderMass)
                    return Status.Incomplete;

                if (orderMass < receivedOrderMass)
                    return Status.Over_Delivered;
            }
            return Status.Complete;
        }

        public static Status GetOrderStatus(IList<AlloyOrder> alloyOrders)
        {
            for (int i = 0; i < alloyOrders.Count; i++)
            {
                if (alloyOrders[i].Status == Status.Incomplete)
                {
                    return Status.Incomplete;
                }

            }

            return Status.Complete;
        }

        public static decimal DiffInPer(decimal orderMass, decimal receivedOrderMass)
        {
            return (Math.Abs(orderMass - receivedOrderMass) / orderMass) * 100;

        }

    }
}
