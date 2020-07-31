exports.up = function(knex) {
    return knex.schema.createTable('imtb_vehicle', function(t) {
        t.increments('Id').unsigned().primary();
        t.string('Owner').notNull();
        t.integer('Hash').unsigned().notNull();
        t.integer('PrimaryColor').unsigned().notNull();
        t.integer('SecondaryColor').unsigned().notNull();
        t.integer('Fuel').unsigned().notNull();
        t.integer('EngineHealth').unsigned().notNull();
    });
};

exports.down = function(knex) {
    return knex.schema.dropTable('imtb_vehicle');
};
