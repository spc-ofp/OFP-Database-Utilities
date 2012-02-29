
/* Drop the function if it already exists (FT is Assembly table-valued function, FS is Assembly scalar-valued function) */
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FnSplitString]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[FnSplitString]
GO

/* Drop the function if it already exists */
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FnSplitDelimitedString]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[FnSplitDelimitedString]
GO

/* Drop the function if it already exists */
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FnSplitNumberString]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[FnSplitNumberString]
GO

/* Drop the assembly if it already exists */
IF EXISTS (SELECT [name] FROM sys.assemblies WHERE [name] = N'OfpDatabaseUtilities')
DROP ASSEMBLY OfpDatabaseUtilities with NO DEPENDENTS;
GO

/* (Re)load the assembly */
/*  NOTE:  This assembly works within the database and so doesn't need any special permissions */
CREATE Assembly OfpDatabaseUtilities From
'C:\Users\coreyc\Documents\Visual Studio 2010\Projects\OfpDatabaseUtilities\OfpDatabaseUtilities\bin\Release\OfpDatabaseUtilities.dll'
GO

-- FnSplitNumberString splits a string into integer values with the default delimiter (,)
CREATE FUNCTION [dbo].[FnSplitNumberString](@value nvarchar(max))
RETURNS TABLE (item int)
AS EXTERNAL NAME [OfpDatabaseUtilities].[Spc.Ofp.UserDefinedFunctions].[SplitStringIntoNumbers]
GO

-- Sample execution for FnSplitNumberString:
-- SELECT * FROM [master].[dbo].[FnSplitNumberString] ('1,2,3,4,5,x')
-- returns 1,2,3,4,5

CREATE FUNCTION [dbo].[FnSplitDelimitedString](@value nvarchar(max), @delimiter nvarchar(255))
RETURNS TABLE (item nvarchar(max))
AS EXTERNAL NAME [OfpDatabaseUtilities].[Spc.Ofp.UserDefinedFunctions].[SplitDelimitedString]
GO

CREATE FUNCTION [dbo].[FnSplitString](@value nvarchar(max))
RETURNS TABLE (item nvarchar(max))
AS EXTERNAL NAME [OfpDatabaseUtilities].[Spc.Ofp.UserDefinedFunctions].[SplitString]
GO