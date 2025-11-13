namespace BanDoGiaDung.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ThemBangLucDau_DaSuaLoi : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccountAddress",
                c => new
                    {
                        account_address_id = c.Int(nullable: false, identity: true),
                        account_id = c.Int(nullable: false),
                        province_id = c.Int(nullable: false),
                        district_id = c.Int(nullable: false),
                        ward_id = c.Int(nullable: false),
                        accountPhoneNumber = c.String(maxLength: 10),
                        accountUsername = c.String(maxLength: 20),
                        content = c.String(maxLength: 50),
                        isDefault = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.account_address_id)
                .ForeignKey("dbo.Districts", t => t.district_id, cascadeDelete: true)
                .ForeignKey("dbo.Provinces", t => t.province_id, cascadeDelete: true)
                .ForeignKey("dbo.Wards", t => t.ward_id, cascadeDelete: true)
                .ForeignKey("dbo.Accounts", t => t.account_id, cascadeDelete: true)
                .Index(t => t.account_id)
                .Index(t => t.province_id)
                .Index(t => t.district_id)
                .Index(t => t.ward_id);
            
            CreateTable(
                "dbo.Accounts",
                c => new
                    {
                        account_id = c.Int(nullable: false, identity: true),
                        password = c.String(),
                        Email = c.String(nullable: false, maxLength: 100),
                        Requestcode = c.String(),
                        Role = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50),
                        Phone = c.String(nullable: false, maxLength: 10),
                        Avatar = c.String(unicode: false, storeType: "text"),
                        create_by = c.String(maxLength: 100),
                        create_at = c.DateTime(nullable: false),
                        update_by = c.String(maxLength: 100),
                        update_at = c.DateTime(nullable: false),
                        status = c.String(maxLength: 1),
                    })
                .PrimaryKey(t => t.account_id);
            
            CreateTable(
                "dbo.Feedback",
                c => new
                    {
                        feedback_id = c.Int(nullable: false, identity: true),
                        account_id = c.Int(nullable: false),
                        product_id = c.Int(nullable: false),
                        genre_id = c.Int(nullable: false),
                        disscount_id = c.Int(nullable: false),
                        content = c.String(),
                        rate_star = c.Int(nullable: false),
                        create_at = c.DateTime(nullable: false),
                        create_by = c.String(nullable: false, maxLength: 100),
                        stastus = c.String(maxLength: 1),
                        update_by = c.String(nullable: false, maxLength: 100),
                        update_at = c.DateTime(),
                    })
                .PrimaryKey(t => t.feedback_id)
                .ForeignKey("dbo.Accounts", t => t.account_id, cascadeDelete: true)
                .ForeignKey("dbo.Product", t => t.product_id, cascadeDelete: true)
                .Index(t => t.account_id)
                .Index(t => t.product_id);
            
            CreateTable(
                "dbo.Product",
                c => new
                    {
                        product_id = c.Int(nullable: false, identity: true),
                        genre_id = c.Int(nullable: false),
                        brand_id = c.Int(nullable: false),
                        disscount_id = c.Int(),
                        product_name = c.String(nullable: false, maxLength: 200),
                        price = c.Double(nullable: false),
                        view = c.Long(nullable: false),
                        buyturn = c.Long(nullable: false),
                        quantity = c.String(nullable: false, maxLength: 10),
                        status = c.String(maxLength: 1),
                        create_by = c.String(nullable: false, maxLength: 100),
                        create_at = c.DateTime(nullable: false),
                        update_by = c.String(maxLength: 100),
                        update_at = c.DateTime(nullable: false),
                        type = c.Int(nullable: false),
                        specifications = c.String(),
                        image = c.String(),
                        description = c.String(),
                    })
                .PrimaryKey(t => t.product_id)
                .ForeignKey("dbo.Brand", t => t.brand_id, cascadeDelete: true)
                .ForeignKey("dbo.Discount", t => t.disscount_id)
                .ForeignKey("dbo.Genre", t => t.genre_id, cascadeDelete: true)
                .Index(t => t.genre_id)
                .Index(t => t.brand_id)
                .Index(t => t.disscount_id);
            
            CreateTable(
                "dbo.Brand",
                c => new
                    {
                        brand_id = c.Int(nullable: false, identity: true),
                        brand_name = c.String(nullable: false, maxLength: 50),
                        create_by = c.String(nullable: false, maxLength: 100),
                        create_at = c.DateTime(nullable: false),
                        update_by = c.String(nullable: false, maxLength: 100),
                        update_at = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.brand_id);
            
            CreateTable(
                "dbo.Discount",
                c => new
                    {
                        disscount_id = c.Int(nullable: false, identity: true),
                        discount_name = c.String(nullable: false, maxLength: 100),
                        discount_star = c.DateTime(nullable: false),
                        discount_end = c.DateTime(nullable: false),
                        discount_price = c.Double(nullable: false),
                        quantity = c.Int(nullable: false),
                        discount_code = c.String(maxLength: 10),
                        create_at = c.DateTime(nullable: false),
                        create_by = c.String(nullable: false, maxLength: 100),
                        update_by = c.String(nullable: false, maxLength: 100),
                        update_at = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.disscount_id);
            
            CreateTable(
                "dbo.Genre",
                c => new
                    {
                        genre_id = c.Int(nullable: false, identity: true),
                        genre_name = c.String(nullable: false, maxLength: 50),
                        create_at = c.DateTime(nullable: false),
                        create_by = c.String(nullable: false, maxLength: 100),
                        update_by = c.String(nullable: false, maxLength: 100),
                        update_at = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.genre_id);
            
            CreateTable(
                "dbo.Oder_Detail",
                c => new
                    {
                        product_id = c.Int(nullable: false),
                        genre_id = c.Int(nullable: false),
                        disscount_id = c.Int(nullable: false),
                        order_id = c.Int(nullable: false),
                        price = c.Double(nullable: false),
                        status = c.String(maxLength: 1),
                        transection = c.String(nullable: false, maxLength: 50),
                        quantity = c.Int(nullable: false),
                        create_by = c.String(nullable: false, maxLength: 20),
                        create_at = c.DateTime(nullable: false),
                        update_by = c.String(nullable: false, maxLength: 20),
                        update_at = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.product_id, t.genre_id, t.disscount_id, t.order_id })
                .ForeignKey("dbo.Order", t => t.order_id, cascadeDelete: true)
                .ForeignKey("dbo.Product", t => t.product_id, cascadeDelete: true)
                .Index(t => t.product_id)
                .Index(t => t.order_id);
            
            CreateTable(
                "dbo.Order",
                c => new
                    {
                        order_id = c.Int(nullable: false, identity: true),
                        orderAddressId = c.Int(),
                        payment_id = c.Int(nullable: false),
                        delivery_id = c.Int(nullable: false),
                        oder_date = c.DateTime(nullable: false),
                        account_id = c.Int(nullable: false),
                        status = c.String(maxLength: 1),
                        order_note = c.String(maxLength: 200),
                        create_at = c.DateTime(nullable: false),
                        total = c.Double(nullable: false),
                        create_by = c.String(nullable: false, maxLength: 100),
                        update_by = c.String(nullable: false, maxLength: 100),
                        update_at = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.order_id)
                .ForeignKey("dbo.Accounts", t => t.account_id, cascadeDelete: true)
                .ForeignKey("dbo.Delivery", t => t.delivery_id, cascadeDelete: true)
                .ForeignKey("dbo.OrderAddress", t => t.orderAddressId)
                .ForeignKey("dbo.Payment", t => t.payment_id, cascadeDelete: true)
                .Index(t => t.orderAddressId)
                .Index(t => t.payment_id)
                .Index(t => t.delivery_id)
                .Index(t => t.account_id);
            
            CreateTable(
                "dbo.Delivery",
                c => new
                    {
                        delivery_id = c.Int(nullable: false, identity: true),
                        delivery_name = c.String(nullable: false, maxLength: 100),
                        price = c.Decimal(nullable: false, storeType: "money"),
                        create_at = c.DateTime(nullable: false),
                        create_by = c.String(nullable: false, maxLength: 20),
                        status = c.String(maxLength: 1),
                        update_by = c.String(nullable: false, maxLength: 20),
                        update_at = c.DateTime(),
                    })
                .PrimaryKey(t => t.delivery_id);
            
            CreateTable(
                "dbo.OrderAddress",
                c => new
                    {
                        orderAddressId = c.Int(nullable: false, identity: true),
                        province_id = c.Int(),
                        district_id = c.Int(),
                        ward_id = c.Int(),
                        orderPhonenumber = c.String(maxLength: 10),
                        orderUsername = c.String(maxLength: 20),
                        content = c.String(maxLength: 150),
                        timesEdit = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.orderAddressId)
                .ForeignKey("dbo.Districts", t => t.district_id)
                .ForeignKey("dbo.Provinces", t => t.province_id)
                .ForeignKey("dbo.Wards", t => t.ward_id)
                .Index(t => t.province_id)
                .Index(t => t.district_id)
                .Index(t => t.ward_id);
            
            CreateTable(
                "dbo.Districts",
                c => new
                    {
                        district_id = c.Int(nullable: false, identity: true),
                        province_id = c.Int(nullable: false),
                        district_name = c.String(nullable: false, maxLength: 50),
                        type = c.String(nullable: false, maxLength: 20),
                    })
                .PrimaryKey(t => t.district_id)
                .ForeignKey("dbo.Provinces", t => t.province_id)
                .Index(t => t.province_id);
            
            CreateTable(
                "dbo.Provinces",
                c => new
                    {
                        province_id = c.Int(nullable: false, identity: true),
                        province_name = c.String(nullable: false, maxLength: 50),
                        type = c.String(nullable: false, maxLength: 20),
                    })
                .PrimaryKey(t => t.province_id);
            
            CreateTable(
                "dbo.Wards",
                c => new
                    {
                        ward_id = c.Int(nullable: false, identity: true),
                        district_id = c.Int(nullable: false),
                        ward_name = c.String(nullable: false, maxLength: 50),
                        type = c.String(nullable: false, maxLength: 20),
                    })
                .PrimaryKey(t => t.ward_id)
                .ForeignKey("dbo.Districts", t => t.district_id)
                .Index(t => t.district_id);
            
            CreateTable(
                "dbo.Payment",
                c => new
                    {
                        payment_id = c.Int(nullable: false, identity: true),
                        payment_name = c.String(nullable: false, maxLength: 50),
                        create_at = c.DateTime(nullable: false),
                        create_by = c.String(nullable: false, maxLength: 20),
                        status = c.String(maxLength: 1),
                        update_by = c.String(nullable: false, maxLength: 20),
                        update_at = c.DateTime(),
                    })
                .PrimaryKey(t => t.payment_id);
            
            CreateTable(
                "dbo.ProductImages",
                c => new
                    {
                        product_img_id = c.Int(nullable: false, identity: true),
                        product_id = c.Int(nullable: false),
                        image = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.product_img_id)
                .ForeignKey("dbo.Product", t => t.product_id, cascadeDelete: true)
                .Index(t => t.product_id);
            
            CreateTable(
                "dbo.ReplyFeedback",
                c => new
                    {
                        rep_feedback_id = c.Int(nullable: false, identity: true),
                        feedback_id = c.Int(nullable: false),
                        account_id = c.Int(nullable: false),
                        content = c.String(),
                        stastus = c.String(maxLength: 1),
                        create_at = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.rep_feedback_id)
                .ForeignKey("dbo.Accounts", t => t.account_id, cascadeDelete: true)
                .ForeignKey("dbo.Feedback", t => t.feedback_id)
                .Index(t => t.feedback_id)
                .Index(t => t.account_id);
            
            CreateTable(
                "dbo.Contact",
                c => new
                    {
                        contact_id = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false),
                        phone = c.String(nullable: false),
                        email = c.String(nullable: false),
                        content = c.String(nullable: false),
                        status = c.String(nullable: false, maxLength: 1),
                        create_by = c.String(nullable: false, maxLength: 20),
                        create_at = c.DateTime(nullable: false),
                        update_by = c.String(nullable: false, maxLength: 20),
                        update_at = c.DateTime(),
                    })
                .PrimaryKey(t => t.contact_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AccountAddress", "account_id", "dbo.Accounts");
            DropForeignKey("dbo.ReplyFeedback", "feedback_id", "dbo.Feedback");
            DropForeignKey("dbo.ReplyFeedback", "account_id", "dbo.Accounts");
            DropForeignKey("dbo.ProductImages", "product_id", "dbo.Product");
            DropForeignKey("dbo.Oder_Detail", "product_id", "dbo.Product");
            DropForeignKey("dbo.Order", "payment_id", "dbo.Payment");
            DropForeignKey("dbo.Order", "orderAddressId", "dbo.OrderAddress");
            DropForeignKey("dbo.OrderAddress", "ward_id", "dbo.Wards");
            DropForeignKey("dbo.Wards", "district_id", "dbo.Districts");
            DropForeignKey("dbo.AccountAddress", "ward_id", "dbo.Wards");
            DropForeignKey("dbo.Districts", "province_id", "dbo.Provinces");
            DropForeignKey("dbo.OrderAddress", "province_id", "dbo.Provinces");
            DropForeignKey("dbo.AccountAddress", "province_id", "dbo.Provinces");
            DropForeignKey("dbo.OrderAddress", "district_id", "dbo.Districts");
            DropForeignKey("dbo.AccountAddress", "district_id", "dbo.Districts");
            DropForeignKey("dbo.Oder_Detail", "order_id", "dbo.Order");
            DropForeignKey("dbo.Order", "delivery_id", "dbo.Delivery");
            DropForeignKey("dbo.Order", "account_id", "dbo.Accounts");
            DropForeignKey("dbo.Product", "genre_id", "dbo.Genre");
            DropForeignKey("dbo.Feedback", "product_id", "dbo.Product");
            DropForeignKey("dbo.Product", "disscount_id", "dbo.Discount");
            DropForeignKey("dbo.Product", "brand_id", "dbo.Brand");
            DropForeignKey("dbo.Feedback", "account_id", "dbo.Accounts");
            DropIndex("dbo.ReplyFeedback", new[] { "account_id" });
            DropIndex("dbo.ReplyFeedback", new[] { "feedback_id" });
            DropIndex("dbo.ProductImages", new[] { "product_id" });
            DropIndex("dbo.Wards", new[] { "district_id" });
            DropIndex("dbo.Districts", new[] { "province_id" });
            DropIndex("dbo.OrderAddress", new[] { "ward_id" });
            DropIndex("dbo.OrderAddress", new[] { "district_id" });
            DropIndex("dbo.OrderAddress", new[] { "province_id" });
            DropIndex("dbo.Order", new[] { "account_id" });
            DropIndex("dbo.Order", new[] { "delivery_id" });
            DropIndex("dbo.Order", new[] { "payment_id" });
            DropIndex("dbo.Order", new[] { "orderAddressId" });
            DropIndex("dbo.Oder_Detail", new[] { "order_id" });
            DropIndex("dbo.Oder_Detail", new[] { "product_id" });
            DropIndex("dbo.Product", new[] { "disscount_id" });
            DropIndex("dbo.Product", new[] { "brand_id" });
            DropIndex("dbo.Product", new[] { "genre_id" });
            DropIndex("dbo.Feedback", new[] { "product_id" });
            DropIndex("dbo.Feedback", new[] { "account_id" });
            DropIndex("dbo.AccountAddress", new[] { "ward_id" });
            DropIndex("dbo.AccountAddress", new[] { "district_id" });
            DropIndex("dbo.AccountAddress", new[] { "province_id" });
            DropIndex("dbo.AccountAddress", new[] { "account_id" });
            DropTable("dbo.Contact");
            DropTable("dbo.ReplyFeedback");
            DropTable("dbo.ProductImages");
            DropTable("dbo.Payment");
            DropTable("dbo.Wards");
            DropTable("dbo.Provinces");
            DropTable("dbo.Districts");
            DropTable("dbo.OrderAddress");
            DropTable("dbo.Delivery");
            DropTable("dbo.Order");
            DropTable("dbo.Oder_Detail");
            DropTable("dbo.Genre");
            DropTable("dbo.Discount");
            DropTable("dbo.Brand");
            DropTable("dbo.Product");
            DropTable("dbo.Feedback");
            DropTable("dbo.Accounts");
            DropTable("dbo.AccountAddress");
        }
    }
}
