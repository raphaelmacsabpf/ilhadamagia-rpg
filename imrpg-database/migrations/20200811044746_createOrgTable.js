
exports.up = function(knex) {
    return knex.schema.createTable('imtb_org', function(t) {
        t.increments('Id').unsigned().primary();
        t.string('Name').notNull();
        t.string('Leader').notNull();
        t.float('SpawnX', 14, 4).notNull().defaultTo(0);
        t.float('SpawnY', 14, 4).notNull().defaultTo(0);
        t.float('SpawnZ', 14, 4).notNull().defaultTo(0);
    });
};

exports.down = function(knex) {
    return knex.schema.dropTable('imtb_org');
};
