@echo off

set "output_file=all.txt"

> "%output_file%" echo.

for /r %%f in (*.cs) do (
    echo Processing %%f
    (
        echo ```
        type "%%f"
        echo ```
        echo.
    ) >> "%output_file%"
)

echo Done. All .cs files have been copied and wrapped in "%output_file%".
