--Creation of DB Procedures

--Table for reference
--CREATE PROCEDURE [dbo].[get_item] @id int
-- CREATE TABLE [store_inventory] (
--   [listing_id] int PRIMARY KEY IDENTITY(1000, 1),
--   [item_id] int,
--   [quantity] int,
--   [unit_price] int,
--   [seller_id] int
-- )
-- GO

--Table for reference
-- CREATE TABLE [users_items] (
--   [id] int,
--   [user_id] int,
--   [quantity] int
-- )
-- GO



CREATE PROCEDURE [dbo].[Sell_Item] @id int, @quantity int, @user_id int, @price int
AS
If Exists(Select TOP 1 1 from user_items where  (id = @id and user_id = @user_id and quantity = @quantity))
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
    If Exists(Select TOP 1 1 from user_items where  (id = @id and user_id = @user_id and quantity > @quantity))
    BEGIN
        PRINT 'BEFORE More TRY';
        Begin Try
        Begin TRANSACTION list_item_more
        --may update duplicate row. if exists
        Update users.items set quantity = (quantity - @quantity)
        where (id = @id and user_id = @user_id and quantity > @quantity);
        INSERT into dbo.store_inventory (item_id, quantity, unit_price, seller_id)values(@id,@quantity,@price,@user_id);
        COMMIT TRANSACTION list_item_more;
        END TRY
        BEGIN CATCH
        PRINT 'IN Catch';
        ROLLBACK TRANSACTION list_item_more;
        THROW 51000, 'Some Error Happened in More.', 1; 
        END CATCH
    END
ELSE THROW 51000, 'May not exist or exist at that quantity.', 1;


