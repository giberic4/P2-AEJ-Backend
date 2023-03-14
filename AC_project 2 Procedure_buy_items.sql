--DROP PROCEDURE [dbo].[buy_item]

CREATE PROCEDURE [dbo].[buy_item] @listing_id int, @quantity int, @buyer_id int, @price int, @item_id int
AS
Declare @total_price int  
set @total_price = @price * @quantity
If Exists(select wallet from users where id = @buyer_id and wallet >= (@price * @quantity))
    If Exists(Select TOP 1 1 from store_inventory where  (listing_id = @listing_id and quantity = @quantity))
        BEGIN
        PRINT 'BEFORE Equal TRY';
        Begin Try
        Begin TRANSACTION buy_Item_equal
        update users set wallet = wallet - @total_price where id = @buyer_id
        DELETE top(1) from store_inventory where (listing_id = @listing_id and quantity = @quantity);
        IF exists (select * from users_items where user_id = @buyer_id and id = @item_id)
            update users_items set quantity = quantity + @quantity where user_id = @buyer_id and id = @item_id
        ELSE 
        INSERT into users_items (id, quantity, user_id)values(@item_id,@quantity,@buyer_id);
        COMMIT TRANSACTION buy_Item_equal;
        END TRY
        BEGIN CATCH
        PRINT 'IN Catch';
        ROLLBACK TRANSACTION buy_Item_equal;
        THROW 51000, 'Some Error Happened in equal', 1; 
        END CATCH
        END
    Else 
        If Exists(Select TOP 1 1 from store_inventory where (listing_id = @listing_id and quantity > @quantity))
            BEGIN
            PRINT 'BEFORE More TRY';
            Begin Try
            Begin TRANSACTION buy_item_more
            update users set wallet = wallet - @total_price where id = @buyer_id
            update top(1) store_inventory set quantity = (quantity - @quantity)
            where (listing_id = @listing_id);
            IF exists (select * from users_items where user_id = @buyer_id and id = @item_id)
                update users_items set quantity = quantity + @quantity where user_id = @buyer_id and id = @item_id
            ELSE 
            INSERT into users_items (id, quantity, user_id)values(@item_id,@quantity,@buyer_id);
            COMMIT TRANSACTION buy_item_more;
            END TRY
            BEGIN CATCH
            PRINT 'IN Catch';
            ROLLBACK TRANSACTION buy_item_more;
            THROW 51000, 'Some Error Happened in More.', 1; 
            END CATCH
            END
        ELSE THROW 51000, 'May not exist or exist at that quantity.', 1;
ELSE THROW 51001, 'Error Non-sufficant funds for this transaction.', 1;



DECLARE @RC int
DECLARE @listing_id int
DECLARE @quantity int
DECLARE @buyer_id int
DECLARE @price int
DECLARE @item_id int

-- TODO: Set parameter values here.

EXECUTE @RC = [dbo].[buy_item] 
   @listing_id = 1001
  ,@quantity = 1
  ,@buyer_id = 5
  ,@price = 200
  ,@item_id = 2
GO



select * from store_inventory
select * from users_items;
select * from users;

-- insert into store_inventory (item_id, quantity, unit_price, seller_id) values (2,1,4000,4)
delete top(1) users_items where user_id = 5