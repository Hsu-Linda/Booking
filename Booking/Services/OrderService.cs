using Booking.Models;
using Booking.Dtos;
using Booking.Repositories;
using System.Transactions;
using Booking.Exceptions;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using System.Timers;
using System.Text.Json;
using AutoMapper;

namespace Booking.Services
{
    public interface IOrderService
    {
        List<Order> GetAllOrders(short memberID);
        short PlaceAnOrder(AddOrderRequestDto addOrderRequest, short memberID);
        OrderDetailResponseDto GetOrderDetail(int orderID, short memberID);
    }
    public class OrderService : IOrderService
    {
        private readonly OrderRepository _orderRepository;
        private readonly TicketRepository _ticketRepository;
        private readonly IMapper _imapper;
        private static readonly object _key = new object();

        public OrderService(
            OrderRepository orderRepository,
            TicketRepository ticketRepository,
            IMapper mapper
            )
        {
            _orderRepository = orderRepository;
            _ticketRepository = ticketRepository;
            _imapper = mapper;
        }
        public List<Order> GetAllOrders(short memberID)
        {
            return _orderRepository.GetAll(memberID);
        }

        public short PlaceAnOrder(AddOrderRequestDto addOrderRequest, short memberID)
        {
            //獲取鎖
            //開始事務
            //查詢庫存 v
            //庫存沒有結束事務 釋放鎖
            //庫存足夠
            //新增訂單 
            //開始計時
            //扣庫存
            // 新增票券
            //結束事務 提交
            //釋放鎖


            lock (_key)
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    short orderID = _orderRepository.CreateOrder(memberID, JsonConvert.SerializeObject(addOrderRequest));
                    foreach (OrderRequest item in addOrderRequest.Items)
                    {
                        short remain = _orderRepository.CheckRemainingQuantity(item.ticketTypeID);
                        if (remain < item.purchaseQuantity)
                        {
                            throw new OrderOperationException("庫存量不足");
                        }
                        _orderRepository.ReduceTicket(item.ticketTypeID, item.purchaseQuantity);
                        _orderRepository.CreateTicket(item.ticketTypeID, memberID, orderID);
                    }
                    scope.Complete();
                    return orderID;
                }
            }

        }

        /// <summary>
        /// totalCount 每一個server固定分一個量 
        /// 可能總共限量50個商品
        /// 這個server分20個商品
        /// 不過可以容許35個進入排隊 以防有人突然不買了
        /// </summary>
        /// <param name="memberID"></param>
        /// <param name="totalCount"></param>
        public void Buy(short memberID, short totalCount)
        {

        }  

        public OrderDetailResponseDto GetOrderDetail(int orderID, short memberID)
        {

            Order order = _orderRepository.GetOrderDetail(orderID, memberID);
            OrderDetailResponseDto result = _imapper.Map<OrderDetailResponseDto>(order);
            result.Items = _ticketRepository.GetTicketsByOrder(orderID, memberID);
            return result;
        }
    }
}
