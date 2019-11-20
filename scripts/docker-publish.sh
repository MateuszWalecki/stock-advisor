DOCKER_ENV=''
DOCKER_TAG=''
DOCKER_ENV=docker

case "$TRAVIS_BRANCH" in
  "master")
    # DOCKER_ENV=production
    DOCKER_TAG=latest
    ;;
  "develop")
    # DOCKER_ENV=development
    DOCKER_TAG=dev
    ;;    
esac

docker login -u $DOCKER_USERNAME -p $DOCKER_PASSWORD

docker build -f ./src/StockAdvisor.Api/Dockerfile.$DOCKER_ENV -t stock-advisor:$DOCKER_TAG ./src/StockAdvisor.Api --no-cache

docker tag stock-advisor:$DOCKER_TAG $DOCKER_USERNAME/stock-advisor:$DOCKER_TAG

docker push $DOCKER_USERNAME/stock-advisor:$DOCKER_TAG