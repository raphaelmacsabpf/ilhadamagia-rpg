exports.up = function(knex) {
    return knex.schema.createTable('imtb_house', function(t) {
        t.increments('Id').unsigned().primary();
        t.string('Owner').notNull();
        t.float('EntranceX', 14, 10).notNull();
        t.float('EntranceY', 14, 10).notNull();
        t.float('EntranceZ', 14, 10).notNull();
        t.integer('PropertyType').notNull().defaultTo(0);
        t.integer('SellState').notNull().defaultTo(0);
        t.integer('Interior').notNull().defaultTo(0);
        t.float('VehiclePositionX', 14, 10).notNull();
        t.float('VehiclePositionY', 14, 10).notNull();
        t.float('VehiclePositionZ', 14, 10).notNull();
        t.float('VehicleHeading', 14, 10).notNull();
    });
};

exports.down = function(knex) {
    return knex.schema.dropTable('imtb_house');
};
