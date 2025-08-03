#!/bin/bash
documentation_generated=false
# Set library version in documentation
# retrieve version in csproj file
csproj="KLVConverter/KLVConverter.csproj"
# Used ascriidoc document
document="doc/documentation.adoc"

echo "Update version in document"
version=$(grep -i -Eo "<Version>(.*)</Version>" $csproj)
version=${version:9:-10}
sed -i -e "s/@VERSION@/$version/g" $document

echo "Version: $version" 
if [ -x "$(command -v docker)" ]; then
    echo "Docker installed"
    # command
    docker run -it -u $(id -u):$(id -g) -v ./doc:/documents/ asciidoctor/docker-asciidoctor asciidoctor-pdf documentation.adoc
    documentation_generated=true
else
    echo "Docker is not installed"
    # command
fi
if [ -x "$(command -v podman)" ] && [ !$documentation_generated ]; then
    echo "Podman installed"
    podman run --rm -v ./doc:/documents/ docker.io/asciidoctor/docker-asciidoctor asciidoctor-pdf documentation.adoc
    documentation_generated=true
else 
    echo "Podman is not installed"
fi
if [ $documentation_generated ]; then
    echo "Documentation generated"
else
    echo "Documentation was not generated cause nor podman, nor docker is installed"
    
fi

# Revert documentation changes
echo "Revert changes"
git checkout $document