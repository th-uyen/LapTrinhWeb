using BanDoGiaDung.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BanDoGiaDung.Services
{
    public class NewsServices
    {
        public List<NewsArticle> GetAllNews()
        {
            return new List<NewsArticle>()
            {
                new NewsArticle
                {
                    Id = 1,
                    Title = "Săn game – Mã giảm đến 500K khi mua máy giặt!",
                    ImageUrl = "/Images/News4.png",
                    PublishDate = DateTime.Now.AddDays(-2),
                    Content =
                    @"
                        <h3>🔥 Cơ hội vàng dành cho hội săn sale</h3>
                        <p>
                            Nâng cấp không gian sống với ưu đãi hấp dẫn từ các thương hiệu hàng đầu.
                            Dịp cuối năm, Điện Máy Xanh tung chương trình <strong>giảm giá đến 500.000đ</strong>
                            dành cho khách hàng mua máy giặt thuộc các thương hiệu: Samsung, LG, Aqua, Toshiba...
                        </p>

                        <img src='/Content/Images/news-mg1.jpg' />

                        <p>
                            Kết hợp ưu đãi thêm khi thanh toán qua ví điện tử Momo, VNPay,
                            giúp bạn tiết kiệm thêm <strong>100.000 – 200.000đ</strong>.
                            Đây là cơ hội vàng để sở hữu những thiết bị giặt sấy, rửa chén hiện đại với chi phí tiết kiệm nhất.
                        </p>
                        
                        <h3>⭐ Danh sách máy giặt hot</h3>
                        <ul>
                            <li>Máy giặt LG Inverter 9Kg – Giảm 2.000.000đ</li>
                            <li>Samsung WW80J54E0BW – Tặng thêm voucher 300K</li>
                            <li>Aqua 8.5Kg – Ưu đãi tới 15%</li>
                        </ul>

                        <p>
                            Chương trình chỉ áp dụng từ <strong>1/11 – 15/11</strong>.
                            Nhanh tay kẻo hết mã giảm giá bé nha 😉
                        </p>
                    "
                },

                new NewsArticle
                {
                    Id = 2,
                    Title = "Đập hộp tủ lạnh LG mới – tiết kiệm điện 40%",
                    ImageUrl ="/Images/News1.png",
                    PublishDate = DateTime.Now.AddDays(-5),
                    Content =
                    @"
                        <h3>🌿 Công nghệ Inverter siêu tiết kiệm</h3>
                        <p>
                            Dòng tủ lạnh LG 2025 sử dụng công nghệ <strong>Linear Inverter</strong>
                            giúp tối ưu điện năng và giảm tiếng ồn.
                        </p>

                        <img src='/Content/Images/news-tl1.jpg' />

                        <p>
                            Thiết kế 2 cửa sang trọng, dung tích lớn,
                            phù hợp với gia đình 3–5 người.
                        </p>

                        <h3>🎁 Ưu đãi mở bán</h3>
                        <p>
                            – Tặng phiếu mua hàng 500K<br/>
                            – Miễn phí vận chuyển<br/>
                            – Bảo hành 24 tháng chính hãng
                        </p>
                    "
                },

                new NewsArticle
                {
                    Id = 3,
                    Title = "Top 5 nồi cơm điện bán chạy nhất năm 2025",
                    ImageUrl = "/Images/News2.png",
                    PublishDate = DateTime.Now.AddDays(-10),
                    Content =
                    @"
                        <h3>🍚 Nồi cơm điện – trợ thủ của mọi gia đình</h3>
                        <p>
                            Dưới đây là danh sách top 5 nồi cơm điện được người dùng đánh giá cao nhất.
                        </p>

                        <img src='/Content/Images/news-nc1.jpg' />

                        <ol>
                            <li>Sharp KS–COM19V – 1.8L – chống dính</li>
                            <li>Philips HD3128 – Công nghệ 3D</li>
                            <li>Sunhouse SHD8615 – Tiết kiệm điện</li>
                            <li>Kangaroo KG565 – Giá tốt!</li>
                            <li>Toshiba RC–18NMF – chống tràn cực tốt</li>
                        </ol>

                        <p>
                            Giá dao động từ <strong>700.000đ – 2.400.000đ</strong>,
                            phù hợp nhiều nhu cầu khác nhau.
                        </p>
                    "
                },

                new NewsArticle
                {
                    Id = 4,
                    Title = "Khai trương chi nhánh mới – Đồng giá 99K",
                    ImageUrl ="/Images/News3.png",
                    PublishDate = DateTime.Now.AddDays(-12),
                    Content =
                    @"
                        <h3>🎉 Khai trương hoành tráng</h3>
                        <p>
                            Chúc mừng khai trương chi nhánh mới tại
                            <strong>Quận 7 – TP. Hồ Chí Minh</strong>.
                        </p>

                        <img src='/Images/News3.png' />

                        <p>
                            Trong 3 ngày đầu khai trương, tất cả sản phẩm gia dụng
                            được bán đồng giá chỉ <strong>99.000đ</strong>.
                        </p>

                        <h3>🎁 Quà tặng kèm</h3>
                        <ul>
                            <li>Voucher 100K</li>
                            <li>Tặng bình giữ nhiệt</li>
                            <li>Miễn phí ship nội thành</li>
                        </ul>
                    "
                },

                new NewsArticle
                {
                    Id = 5,
                    Title = "BLACK FRIDAY - THÁNG SIÊU SALE - LỊCH SALE THÁNG 11 - KHAI MÀN MÙA MUA SẮM LỚN NHẤT NĂM",
                    ImageUrl ="/Images/News5.png",
                    PublishDate = DateTime.Now.AddDays(-12),
                    Content =
                    @"
                        <h3>🎉 Khai màn mùa mua sắm lớn nhất năm 2025 🎉</h3>
                        <h3>1.Thời gian khuyến mãi: Từ 01/11 - 30/11/2025 </h3>

                        <h3>2.Nội dung chương trình:</h3>
                        <p>Cửa hàng chúng tôi  mang đến chương trình ""BLACK FRIDAY – THÁNG SIÊU SALE – TUNG DEAL BÙNG NỔ"". Tiếp tục hành trình săn deal ""không giới hạn""
                            từ hệ sinh thái MWG: AVAKids, Thế Giới Di Động, Điện Máy XANH,
                            TopZone - hàng ngàn ưu đãi cực khủng đang chờ bạn!<p>

                        <img src='/Images/News6.png' />

                        <h3>3.🎁 LỊCH SALE THÁNG 11 – KHAI MÀN MÙA MUA SẮM LỚN NHẤT NĂM</h3>
                        <ul>
                            <li><strong>08/11 - 11/11:</strong> 11/11 SALE NGÀY ĐÔI - DEAL VÔ ĐỐI</li>
                            <li><strong>15/11 - 17/11:</strong> GIỮA THÁNG SĂN SALE - CHỚP DEAL THẦN TỐC</li>
                            <li><strong>19/11 - 20/11:</strong> TRI ÂN CÔ THẦY - DEAL TỐT TRAO TAY</li>
                            <li><strong>22/11 - 25/11:</strong> CUỐI THÁNG SALE TO - GIÁ TỐT KHỎI LO</li>
                            <li><strong>28/11 - 30/11:</strong> THÁNG SIÊU SALE - TUNG DEAL BÙNG NỔ</li>
                        </ul>

                        <h3>4.FLASH SALE GIÁ SIÊU SỐC</h3>
                        <ul>
                            <li><strong>5</strong> khung giờ mỗi ngày - giá giảm sâu siêu bất ngờ.</li>
                            <li>Số lượng có hạn - chốt nhanh kẻo lỡ!</li>
                        </ul>
                        
                        <h3>5.ĐẶC BIỆT, DEAL XỊN DÀNH RIÊNG CHO THÁNG 11:</h3>
                            <ul>
                                <li>Giảm <strong>100.000Đ</strong> khi mua sản phẩm trực tiếp tại cửa hàng, áp dụng cho đơn hàng từ 1 triệu.</li>
                                <li>Giảm <strong>300.000Đ</strong> cho đơn từ 1 - dưới 3 triệu.</li>
                                <li>Giảm <strong>600.000Đ</strong> cho đơn từ 3 triệu trở lên.</li>
                            </ul>
                    "
                },
                 new NewsArticle
                {
                    Id = 6,
                    Title = "THÁNG CỦA NÀNG - DEAL NGẬP TRÀN LỊCH SALE THÁNG 10 - DEAL HOT KHẮP SÀN",
                    ImageUrl ="/Images/News7.png",
                    PublishDate = DateTime.Now.AddDays(-20),
                    Content =
                    @"
                        <h3>1.Thời gian khuyến mãi:  Từ 01/11 - 31/11/2025 </h3>

                        <h3>2.Nội dung chương trình:</h3>
                        <p>Cửa hàng chúng tôi   chương trình ""THÁNG CỦA NÀNG - DEAL NGẬP TRÀN"". 
                           Tiếp tục hành trình săn deal “không giới hạn” từ hệ sinh thái MWG: AVAKids, 
                           Thế Giới Di Động, Điện Máy XANH, TopZone - hàng ngàn ưu đãi cực khủng đang chờ bạn!<p>

                        <img src='/Images/News7-1.png' />

                        <h3>3.🎁 LỊCH SALE THÁNG 11 – KHAI MÀN MÙA MUA SẮM LỚN NHẤT NĂM</h3>
                        <ul>
                            <li><strong>08/11 - 11/11:</strong> 11/11 SALE NGÀY ĐÔI - DEAL VÔ ĐỐI</li>
                            <li><strong>15/11 - 17/11:</strong> GIỮA THÁNG SĂN SALE - XẢ LÁNG SĂN DEAL</li>
                            <li><strong>19/11 - 20/11:</strong> TRI ÂN CÔ THẦY - DEAL TỐT TRAO TAY</li>
                            <li><strong>22/11 - 25/11:</strong> TIỆC SALE CUỐI THÁNG - DEAL SIÊU HOÀNH TRÁNG</li>
                            <li><strong>28/11 - 30/11:</strong> VUI HALLOWEEN - SALE CỰC ĐỈNH</li>
                        </ul>

                        <h3>4.FLASH SALE GIÁ SIÊU SỐC</h3>
                        <ul>
                            <li><strong>5</strong> khung giờ mỗi ngày - giá giảm sâu siêu bất ngờ.</li>
                            <li>Số lượng có hạn - chốt nhanh kẻo lỡ!</li>
                        </ul>
                        
                        <h3>5.ĐẶC BIỆT, DEAL XỊN DÀNH RIÊNG CHO THÁNG 11:</h3>
                            <ul>
                                <li>Giảm <strong>100.000Đ</strong> khi mua sản phẩm trực tiếp tại cửa hàng, áp dụng cho đơn hàng từ 1 triệu.</li>
                                <li>Giảm <strong>300.000Đ</strong> cho đơn từ 1 - dưới 3 triệu.</li>
                                <li>Giảm <strong>600.000Đ</strong> cho đơn từ 3 triệu trở lên.</li>
                            </ul>
                        <h3>6.ƯU ĐÃI VẬN CHUYỂN </h3>
                            <ul>
                                <li>Freeship cho đơn hàng từ 200.000đ trong bán kính 10km.</li>
                                <li>Đặc biệt, freeship đơn hàng từ 0đ trong bán kính 10km cho các Campaign tháng 10 và Mini-Campaign Thứ 4 Freeship diễn ra Hàng tuần.</li>
                                <li>Với các đơn ngoài 10km, phí phụ thu: 5.000đ/km.</li>
                            </ul>
                        <p>Nhanh tay săn deal cực khủng, mua sắm cực thích chỉ có tại MWG Shop! Truy cập ngay MWG Shop trong app Quà Tặng VIP để săn ưu đãi mỗi ngày!</p>
                    "
                },

            };
        }



        public NewsArticle GetById(int id)
        {
            return GetAllNews().FirstOrDefault(x => x.Id == id);
        }



    }
}