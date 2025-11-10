const db_client = require('./db/db_client.js')
const restAPI = require('./network/rest_server.js')

async function main() {
    await db_client.InitDatabase();

    restAPI.listen(8080, () => {
    console.log('Server is listening on port 8080...');
    })
}

main()
  .then(async () => {
    await db_client.prisma.$disconnect()
  })
  .catch(async (e) => {
    console.error(e)
    await db_client.prisma.$disconnect()
    process.exit(1)
  })