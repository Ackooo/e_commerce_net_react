BEGIN TRY
BEGIN TRAN T1
	DECLARE @Schema NVARCHAR(20) = ''
	DECLARE @sql NVARCHAR(MAX)='';

	SELECT @sql = @sql + ' ALTER INDEX ' + QUOTENAME(I.name) + ' ON ' + QUOTENAME(SCHEMA_NAME(T.schema_id)) + '.' + QUOTENAME(T.name) + ' REBUILD; '
	FROM sys.indexes I INNER JOIN sys.tables T ON I.object_id = T.object_id
	WHERE I.type_desc = 'NONCLUSTERED' and T.schema_id = (SELECT schema_id FROM sys.schemas WHERE name = @Schema)
	AND I.[name] IS NOT NULL AND I.is_disabled = 0

	EXEC(@sql);
	PRINT 'All indexes rebuilt successfully'

COMMIT TRAN T1
END TRY
BEGIN CATCH
	PRINT 'Error while executing!'
	ROLLBACK TRAN T1
	SELECT ERROR_MESSAGE() AS Msg, ERROR_NUMBER() AS Num
END CATCH