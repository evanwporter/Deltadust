@echo off

REM Check if an argument is provided, if not, default to the current directory
if "%~1"=="" (
    set "base_dir=%cd%"
) else (
    set "base_dir=%~1"
)

set "output_file=all.txt"

REM Clear the output file
> "%output_file%" echo.

REM Recursively search for .cs files in the specified (or default) base directory
for /r "%base_dir%" %%f in (*.cs) do (
    echo Processing %%f
    (
        echo ```
        type "%%f"
        echo ```
        echo.
    ) >> "%output_file%"
)

echo Done. All .cs files from "%base_dir%" have been copied and wrapped in "%output_file%".
