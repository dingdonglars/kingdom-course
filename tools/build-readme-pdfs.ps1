# Renders the EN + PT Athos READMEs to PDF via pandoc + xelatex.
# Run from the course-repo root:  powershell -File tools/build-readme-pdfs.ps1
#
# Output is gitignored (*.pdf) — local-only artefact, regenerated on demand.

$ErrorActionPreference = 'Stop'

$here = Split-Path -Parent $MyInvocation.MyCommand.Path
$root = Split-Path -Parent $here
Push-Location $root
try {
    pandoc README.md    --defaults=tools/readme-pdf.yaml                 -o README.pdf
    pandoc README.pt.md --defaults=tools/readme-pdf.yaml -V lang=pt-BR   -o README.pt.pdf
    Write-Host "Wrote README.pdf and README.pt.pdf"
}
finally {
    Pop-Location
}
