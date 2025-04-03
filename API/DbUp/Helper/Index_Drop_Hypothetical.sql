BEGIN TRY
BEGIN TRAN T1
	DECLARE @Schema NVARCHAR(10) = ''
	DECLARE @sql VARCHAR(MAX) = ''

	SELECT
		@sql = sql + 'DROP INDEX IF EXISTS [' + i.name + '] ON [' + @Schema + '].[' + t.name + ']' + CHAR(13) + CHAR(10)
	FROM
		sys.indexes i
		INNER JOIN sys.tables T
		ON i.object_id = t.object_id
	WHERE
		i.is_hypothetical = 1
		--AND i.type = 2 -- NONCLUSTERED
		--AND t.name in ('')
		--AND i.is_primary_key = 0

	EXECUTE sp_sqlexec @sql
	PRINT 'All hypothetical indexes dropped successfully'

COMMIT TRAN T1
END TRY
BEGIN CATCH
	PRINT 'Error while executing!'
	ROLLBACK TRAN T1
	SELECT ERROR_MESSAGE() AS Msg, ERROR_NUMBER() AS Num
END CATCH