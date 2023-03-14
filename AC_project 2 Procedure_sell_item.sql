--Creation of DB Procedure Sell_item


--DROP PROCEDURE [dbo].[Sell_Item]

CREATE PROCEDURE [dbo].[Sell_Item] @id int, @quantity int, @user_id int, @price int
AS
If Exists(Select TOP 1 1 from users_items where  (id = @id and user_id = @user_id and quantity = @quantity))
BEGIN
    PRINT 'BEFORE Equal TRY';
    Begin Try
    Begin TRANSACTION list_item_equal
    DELETE top(1) from users_items where (id = @id and user_id = @user_id and quantity = @quantity);
    INSERT into dbo.store_inventory (item_id, quantity, unit_price, seller_id)values(@id,@quantity,@price,@user_id);
    COMMIT TRANSACTION list_item_equal;
    END TRY
    BEGIN CATCH
    PRINT 'IN Catch';
    ROLLBACK TRANSACTION list_item_equal;
    THROW 51000, 'Some Error Happened in equal', 1; 
    END CATCH
END
Else 
    If Exists(Select TOP 1 1 from users_items where  (id = @id and user_id = @user_id and quantity > @quantity))
    BEGIN
        PRINT 'BEFORE More TRY';
        Begin Try
        Begin TRANSACTION list_item_more
        --may update duplicate row. if exists
        update top(1) users_items set quantity = (quantity - @quantity)
        where (id = @id and user_id = @user_id and quantity > @quantity);
        INSERT INTO [store_inventory] (item_id, quantity, unit_price, seller_id)values(@id,@quantity,@price,@user_id);
        COMMIT TRANSACTION list_item_more;
        END TRY
        BEGIN CATCH
        PRINT 'IN Catch';
        ROLLBACK TRANSACTION list_item_more;
        THROW 51000, 'Some Error Happened in More.', 1; 
        END CATCH
    END
ELSE THROW 51000, 'May not exist or exist at that quantity.', 1;




--Testing sell Items

DECLARE @RC int
DECLARE @id int
DECLARE @quantity int
DECLARE @user_id int
DECLARE @price int

EXECUTE @RC = [dbo].[Sell_Item] 
   @id = 2 
  ,@quantity = 2
  ,@user_id = 2
  ,@price = 200
GO

Select * from users;
select * from users_items;
Select * from store_inventory;

truncate table store_inventory;
