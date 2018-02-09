#!/bin/bash
set -e

DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
COMMAND=apply

while [[ $# -gt 0 ]]; do
  case $1 in
    -v|--version)
      VERSION="$2"
      shift # past argument
      shift # past value
      ;;
    -e|--environment)
      ENVIRONMENT="$2"
      shift # past argument
      shift # past value
      ;;
    -n|--namespace)
      NAMESPACE="$2"
      shift # past argument
      shift # past value
      ;;
    -r|--replace)
      COMMAND="replace --force --cascade"
      shift # past argument
      ;;
    *)    # unknown option
      echo "Invalid argument" >&2
      exit 1
      ;;
  esac
done

if [[ -z "$VERSION" || -z "$ENVIRONMENT" ]]; then
  echo "VERSION and ENVIRONMENT required"
  exit 1
fi

function prompt_create_namespace {
  while true; do
    read -p "Do you wish to create the namespace \"${NAMESPACE}\"? (y/n) " yn
    case $yn in
        [Yy]* ) return 1;;
        [Nn]* ) exit;;
        * ) echo "Please answer yes or no.";;
    esac
  done
}

echo Current context: $(kubectl config current-context)

NAMESPACE=${NAMESPACE:-facility-${ENVIRONMENT}}
(kubectl get namespace ${NAMESPACE} > /dev/null 2>&1) || prompt_create_namespace || kubectl create namespace ${NAMESPACE}

cat ${DIR}/${ENVIRONMENT}/*.yaml | \
  sed -e "s/\$ENVIRONMENT/${ENVIRONMENT}/g" | \
  sed -e "s/\$VERSION/${VERSION}/g" | \
  kubectl --namespace ${NAMESPACE} ${COMMAND} -f -
kubectl --namespace ${NAMESPACE} rollout status deploy/generator-api
