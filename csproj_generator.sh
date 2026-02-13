#!/bin/bash

set -e
set -o pipefail
set -u

PATH_STEAMAPP="${HOME}/.local/share/Steam/steamapps/common"
PATH_LOGIC_WORLD="${PATH_STEAMAPP}/Logic World"

OUT_FILE="Pix_logic_utils.csproj"

cat << EOF > "${OUT_FILE}"
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net6.0</TargetFramework>
    <LangVersion>latest</LangVersion>
    <ImplicitUsings>disable</ImplicitUsings>
    <EnableDefaultCompileItems>false</EnableDefaultCompileItems>
  </PropertyGroup>

  <ItemGroup>
    <Compile Include="${PATH_LOGIC_WORLD}/GameData/Ecconia_*/**/*.cs" />
    <Compile Include="${PATH_LOGIC_WORLD}/GameData/Pix_logic_utils/src/shared/*.cs" />
    <Compile Include="logic_utils/src/**/*.cs"/>
    <Compile Include="mod_manager/src/**/*.cs"/>
  </ItemGroup>

  <ItemGroup>
    <PackageReference Include="System.IO.Compression" Version="4.3.0" />
  </ItemGroup>

  <ItemGroup>
EOF

find "${PATH_LOGIC_WORLD}" -type f -name "*.dll" | while read -r file; do
	if grep -i -e "cache" <<<"${file}" > /dev/null; then
		continue
	fi
	cat <<EOF >> "${OUT_FILE}"
    <Reference Include="$(basename "${file%.dll}")">
	  <SpecificVersion>false</SpecificVersion>
	  <Private>false</Private>
      <HintPath>${file}</HintPath>
    </Reference>
EOF
done

cat <<EOF >> "${OUT_FILE}"
  </ItemGroup>
</Project>
EOF
